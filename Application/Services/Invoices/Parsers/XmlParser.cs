
using System.Collections.Concurrent;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;

namespace HealthInvoice.Application.Services.Invoices.Parsers;

public class XmlParser<T>(
    ILogger<XmlParser<T>> logger) : IXmlParser<T>
{

    private static readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new();

    public async Task<T?> ParseToEntityAsync(Stream xmlStream)
    {
        ArgumentNullException.ThrowIfNull(xmlStream);

        try
        {
            var serializer = _serializers.GetOrAdd(
                typeof(T),
                type => new XmlSerializer(type));

            return (T?)serializer.Deserialize(xmlStream);
         
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка десериализации XML для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
        catch (XmlException ex)
        {
            logger.LogError(
                ex,
                "XML‑ошибка при десериализации для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Операция десериализации XML отменена для типа {TypeName}",
                typeof(T).Name);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при десериализации XML для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
    }
    public async Task<MemoryStream> ParseToXmlAsync(T objectContent)
    {
        try
        {
            var ms = new MemoryStream();

            var serializer = _serializers.GetOrAdd(
                typeof(T),
                type => new XmlSerializer(type));

            await using var writer = new StreamWriter(
                ms,
                Encoding.UTF8,
                leaveOpen: true);

            serializer.Serialize(writer, objectContent);

            ms.Position = 0;
            return ms;
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка сериализации XML для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
        catch (XmlException ex)
        {
            logger.LogError(
                ex,
                "XML‑ошибка при сериализации для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при сериализации XML для типа {TypeName}: {Message}",
                typeof(T).Name,
                ex.Message);
            throw;
        }
    }
}
