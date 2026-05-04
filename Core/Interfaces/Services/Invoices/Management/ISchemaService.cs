using System.Xml.Schema;

namespace HealthInvoice.Core.Interfaces.Services.Invoices.Management;

/// <summary>
/// Сервис для работы с XML‑схемами (XSD), используемыми для валидации XML‑файлов.
/// Предоставляет доступ к кэшированным схемам по ключу для последующей проверки соответствия XML‑документов.
/// </summary>
public interface ISchemaService
{
    /// <summary>
    /// Загружает XML‑схему (XSD) по указанному ключу.
    /// Схема используется для валидации структуры XML‑файлов на соответствие стандарту.
    /// </summary>
    /// <param name="schemaKey">
    /// Уникальный идентификатор схемы, по которому она хранится в хранилище.
    /// </param>
    /// <returns>
    /// Объект XmlSchemaSet, содержащий загруженную XSD‑схему.
    /// Используется для валидации XML‑файлов через XmlReaderSettings.Schemas.
    /// </returns>
    /// <remarks>
    /// Реализация сервиса может использовать кэширование схем для повышения производительности.
    /// При повторном запросе с тем же schemaKey возвращается уже загруженная схема.
    /// </remarks>
    XmlSchemaSet LoadXsdSchemaSet(string schemaKey);

}
