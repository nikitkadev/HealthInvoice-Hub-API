using System.Xml.Schema;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Interfaces.Services.Invoices.Management;

namespace HealthInvoice.Application.Services.Invoices.Management;

public class SchemaService(
    ILogger<SchemaService> logger,
    ISchemaSource schemaSource) : ISchemaService
{
    public XmlSchemaSet LoadXsdSchemaSet(string schemaKey)
    {
        if (string.IsNullOrEmpty(schemaKey))
            throw new ArgumentException("schemaKey не может быть пустым", nameof(schemaKey));

        var schemaSet = new XmlSchemaSet();
        
        try
        {
            using var schemaStream = schemaSource.OpenSchemaStream(schemaKey);

            if (schemaStream == null)
            {
                logger.LogError(
                    "Не удалось открыть поток для схемы {SchemaKey}", 
                    schemaKey);

                throw new FileNotFoundException(
                    $"Схема не найдена: {schemaKey}");
            }

            var schema = XmlSchema.Read(
                schemaStream,
                (sender, e) =>
                {
                    logger.LogWarning(
                        "Предупреждение при чтении xsd-схемы {Path}: {Message}",
                schemaKey,
                e.Message);
                });

            if (schema is null)
            {
                logger.LogWarning(
                    "Не удалось получить схему из {SchemaKey}", 
                    schemaKey);

                throw new InvalidOperationException(
                    $"Не удалось получить схему из {schemaKey}");
            }

            schemaSet.Add(schema);
            schemaSet.Compile();

            return schemaSet;
        }
        catch (XmlSchemaException ex)
        {
            logger.LogError(
                ex,
                "Ошибка валидации XSD‑схемы {SchemaKey}: {Message} на строке {LineNumber}, позиции {LinePosition}",
                schemaKey,
                ex.Message,
                ex.LineNumber,
                ex.LinePosition);
            throw new InvalidOperationException(
                $"Ошибка схемы {schemaKey}: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            logger.LogError(
                ex,
                "IO‑ошибка при загрузке схемы {SchemaKey}: {Message}",
                schemaKey,
                ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Неожиданная ошибка при загрузке схемы {SchemaKey}: {ErrorType} — {Message}",
                schemaKey,
                ex.GetType().Name,
                ex.Message);
            throw new InvalidOperationException(
                $"Критическая ошибка при загрузке схемы {schemaKey}", ex);
        }
    }
}
