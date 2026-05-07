using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Service;
using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Core.Interfaces.Repository.Journals;

/// <summary>
/// Репозиторий для выполнения CRUD‑операций с журналом ФЛК (таблица journal_flk).
/// Предоставляет методы для чтения записей по коду организации и обновления статусов счетов.
/// </summary>
public interface ILkJournalRepository
{
    /// <summary>
    /// Получить записи из журнала по коду организации
    /// </summary>
    /// <param name="sorting">Модель с параметрами сортировки</param>
    /// <param name="filters">Модель примененных фильтров</param>
    /// <param name="organizationCode">Код организации</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken ">Токен отмены</param>
    /// <returns>Список записей из журнала по указанному коду организации</returns>
    Task<(List<LogicControlJournalEntity>, int)> GetRecordsAsync(
        SortingRequest sorting,
        LogicControlJournalFilters filters,
        string organizationCode,
        int skip, 
        int take,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Обновление статуса счетов в указанном журнале
    /// </summary>
    /// <param name="schetUids">Список uid'ов счетов, у которых нужно обновить статус</param>
    /// <param name="status">Задаваемый статус</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cs">Токен отмены</param>
    /// <returns></returns>
    Task UpdateInvoiceStatusAsync(
        List<int> schetUids,
        short status,
        JournalType journalType = JournalType.None,
        CancellationToken cs = default);


    /// <summary>
    /// Получение статуса проверки МЭК для файла.
    /// </summary>
    /// <param name="filename">Имя файла, чей статус надо получить.</param>
    /// <returns>Статус в виде числа.</returns>
    Task<short?> GetStatusByFilenameAsync(
        string filename,
        JournalType journalType,
        CancellationToken cancellationToken = default);
}
