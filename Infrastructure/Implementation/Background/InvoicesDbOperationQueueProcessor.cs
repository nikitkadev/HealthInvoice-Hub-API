using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Interfaces.Managers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;

namespace HealthInvoice.Infrastructure.Implementation.Background;

public class InvoicesDbOperationQueueProcessor(
    ILogger<InvoicesDbOperationQueueProcessor> logger,
    IServiceScopeFactory scopeFactory,
    IQueuePublisher<InvoiceDbOperationMessage> publisher) : BackgroundService
{
    private readonly int _maxConcurrency = 5;
    private readonly TimeSpan _startupDelay = TimeSpan.FromMilliseconds(100);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var tasks = new List<Task>();

            await foreach (var item in publisher.Reader.ReadAllAsync(cancellationToken))
            {
                if (tasks.Count >= _maxConcurrency)
                {
                    logger.LogInformation("[HealthInvoice Hub] Ожидаем освобождения слота!");

                    var completed = await Task.WhenAny(tasks);
                    tasks.Remove(completed);
                }

                switch (item.DbOperation)
                {
                    case DbOperation.Insert:

                        await Task.Delay(_startupDelay, cancellationToken);

                        tasks.Add(ProcessInsertInvoiceDataAsync(
                            item.FilePath,
                            item.Uploader,
                            item.JournalType,
                            cancellationToken));

                        break;

                    case DbOperation.Update:
                        
                        await Task.Delay(_startupDelay, cancellationToken);

                        tasks.Add(ProcessRewriteInvoiceDataAsync(
                            item.FilePath,
                            item.Uploader,
                            item.SchetUid,
                            item.JournalType,
                            cancellationToken));

                        break;
                }
            }

            await Task.WhenAll(tasks);

        }
        catch (OperationCanceledException)
        {
            logger.LogError(
                "Операция записи счета в базу через фоновый сервис InvoicesDbInsertingQueueProcessor отменена токеном");

            throw;
        }
    }

    private async Task ProcessInsertInvoiceDataAsync(
        string filePath,
        string uploader,
        JournalType journalType,
        CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var invoiceManager = scope.ServiceProvider.GetRequiredService<IInvoiceManager>();

        await invoiceManager.RegisterInvoiceAsync(filePath, uploader, journalType, cancellationToken);

        logger.LogInformation("[HealthInvoice Hub] Счет записан в базу данных!");
    }

    private async Task ProcessRewriteInvoiceDataAsync(
        string filePath,
        string uploader,
        int? schetUid,
        JournalType journalType,
        CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var invoiceManager = scope.ServiceProvider.GetRequiredService<IInvoiceManager>();

        await invoiceManager.UpdateInvoiceAsync(filePath, uploader, schetUid ?? -1, journalType, cancellationToken);

        logger.LogInformation("[HealthInvoice Hub] Счет записан в базу данных!");
    }
}