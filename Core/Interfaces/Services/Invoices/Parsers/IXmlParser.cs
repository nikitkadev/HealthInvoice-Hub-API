namespace HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;

/// <summary>
/// Сервис для парсинга данных из XML‑файлов в C#‑классы и обратного преобразования.
/// </summary>
/// <typeparam name="TEntity">Тип объекта, в который будет преобразован XML‑файл или из которого будет создан XML.</typeparam>
public interface IXmlParser<TEntity>
{
    /// <summary>
    /// Парсит XML‑данные из потока в объект указанного типа.
    /// </summary>
    /// <param name="xmlContent">Поток с XML‑данными для парсинга.</param>
    /// <returns>Объект типа TEntity с данными, извлечёнными из XML.</returns>
    Task<TEntity?> ParseToEntityAsync(Stream xmlStream);

    /// <summary>
    /// Преобразует объект в XML‑представление и возвращает его в виде потока.
    /// </summary>
    /// <param name="objectContent">Объект для сериализации в XML.</param>
    /// <returns>MemoryStream с XML‑данными, содержащими сериализованный объект.</returns>
    Task<MemoryStream> ParseToXmlAsync(TEntity objectContent);

    
}
