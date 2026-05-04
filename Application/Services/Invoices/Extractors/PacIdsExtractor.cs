using System.Data;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Interfaces.Services.Invoices.Extractors;

namespace HealthInvoice.Application.Services.Invoices.Extractors;

public class PacIdsExtractor(
    ILogger<PacIdsExtractor> logger) : IPacIdsExtractor
{
    public async Task<List<string?>> ExtractPacIdsAsync(
       Stream account,
       string targetTable,
       CancellationToken cancellationToken = default)
    {
        if (account is null)
            throw new ArgumentNullException(
                nameof(account), 
                "Поток account не может быть null");

        if (string.IsNullOrEmpty(targetTable))
            throw new ArgumentException(
                "targetTable не может быть пустой строкой", 
                nameof(targetTable));

        try
        {
            var doc = await XDocument.LoadAsync(account, LoadOptions.None, cancellationToken);

            var result = doc.Descendants(targetTable)
                .Select(pac => pac.Element("ID_PAC")?.Value?.Trim())
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            return result; 
        }
        catch (XmlException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при загрузке или парсинге XML из потока account для таблицы {Table}: {Message}",
                targetTable,
                ex.Message);

            throw; 
        }
        catch (NullReferenceException ex)
        {
            logger.LogError(
                ex,
                "Неожиданная NullReferenceException при обработке XML для таблицы {Table}: {Message}",
                targetTable,
                ex.Message);

            throw; 
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Операция ExtractPacIdsAsync отменена по токену отмены");

            throw; 
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Непредвиденная ошибка при извлечении ID_PAC для таблицы {Table}: {Message}",
                targetTable,
                ex.Message);

            throw; 
        }
    }
}
