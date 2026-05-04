using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;

namespace HealthInvoice.Infrastructure.Implementation.Services.Invoices.Publishers;

public class DbLogicControlQueuePublisher(
    ILogger<DbLogicControlQueuePublisher> logger,
    Channel<InvoiceLogicControlMessage> channel) : IQueuePublisher<InvoiceLogicControlMessage>
{
    public async Task PublishAsync(
        List<InvoiceLogicControlMessage> invoices,
        string queueName = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Parallel.ForEachAsync(
                invoices,
                async (invoice, cancellationToken) =>
                {
                    await channel.Writer.WriteAsync(
                        invoice,
                        cancellationToken);

                });
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(
                ex,
                "Операция публикации счетов в очередь отменена токеном");

            throw;
        }
    }

    public ChannelReader<InvoiceLogicControlMessage> Reader => channel.Reader;
}