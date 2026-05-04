namespace HealthInvoice.Core.Interfaces.Services.Invoices.Extractors;

/// <summary>
/// Интерфейс для извлечения идентификаторов PAC (Patient Account Code) из потока данных.
/// Предоставляет метод для асинхронного извлечения списка идентификаторов из учётной записи
/// с указанием целевой таблицы и возможностью отмены операции.
/// </summary>
public interface IPacIdsExtractor
{
    /// <summary>
    /// Асинхронно извлекает идентификаторы PAC из потока данных.
    /// </summary>
    /// <param name="account">Поток данных, содержащий информацию об учётной записи.</param>
    /// <param name="targetTable">Название целевой таблицы, из которой извлекаются данные.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список извлечённых идентификаторов PAC.</returns>
    Task<List<string?>> ExtractPacIdsAsync(
        Stream account,
        string targetTable,
        CancellationToken cancellationToken = default);
}