using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;
using HealthInvoice.Core.Interfaces.Repository.Invoices;

namespace HealthInvoice.Infrastructure.Implementation.Background;

public class InvoicesLogicalControlQueueProcessor(
    ILogger<InvoicesLogicalControlQueueProcessor> logger,
    IQueuePublisher<InvoiceLogicControlMessage> schetUidsPublisher,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly int _maxConcurrency = 2;
    private readonly TimeSpan _startupDelay = TimeSpan.FromMilliseconds(100);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var logicControlTasks = new List<Task>();

        await foreach(var item in schetUidsPublisher.Reader.ReadAllAsync(cancellationToken))
        {

            if(logicControlTasks.Count >= _maxConcurrency)
            {
                logger.LogInformation("[HealthInvoice Hub] Ожидаем выполнение задачи!");
                var completed = await Task.WhenAny(logicControlTasks);
                logicControlTasks.Remove(completed);
            }

            await Task.Delay(_startupDelay, cancellationToken);
            logger.LogInformation("[HealthInvoice Hub] Ставим задачу в очередь!");
            logicControlTasks.Add(ProcessItemAsync(item, cancellationToken));
        }

        await Task.WhenAll(logicControlTasks);
    }

    private async Task ProcessItemAsync(InvoiceLogicControlMessage item, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var invoiceManager = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
        
        await invoiceManager.PerformLogicControlAsync(item.SchetUid, item.JournalType,cancellationToken);

        logger.LogInformation("[HealthInvoice Hub] МЭК проведен!");
    }
}