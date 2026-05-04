using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;

namespace HealthInvoice.Infrastructure.Implementation.Services.Invoices.Publishers;

public class DbOperationQueuePublisher(
    ILogger<DbOperationQueuePublisher> logger,
    Channel<InvoiceDbOperationMessage> channel) : IQueuePublisher<InvoiceDbOperationMessage>
{
    public async Task PublishAsync(
        List<InvoiceDbOperationMessage> invoices, 
        string queueName = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach(var invoice in invoices)
            {
                await channel.Writer.WriteAsync(invoice, cancellationToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(
                ex,
                "Операция публикации счетов в очередь отменена токеном");

            throw;
        }
    }

    public ChannelReader<InvoiceDbOperationMessage> Reader => channel.Reader;
}