using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Core.Interfaces.Repository.Invoices;

/// <summary>
/// Репозиторий для работы со счетами в различных журналах (SMORX, Inogorod).
/// Обеспечивает операции вставки, удаления, обновления статуса и логического контроля.
/// </summary>
public interface IInvoiceRepository
{
    /// <summary>
    /// Вставка счета в БД
    /// </summary>
    /// <param name="models">Кортеж сущностей: ZL_LIST_ENTITY (основные данные счёта, H-формат) и PERS_LIST_ENTITY (персональные данные, L-формат)</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task InsertInvoiceAsync(
        (ZlListEntity, PersListEntity) models,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Удаление счета из БД
    /// </summary>
    /// <param name="schetUid">Uid удаляемого счета</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken ">Токен отмены</param>
    /// <returns></returns>
    Task RemoveInvoiceAsync(
        int schetUid,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Перезапись счета в БД
    /// </summary>
    /// <param name="schetUid">Uid перезаписаного счета</param>
    /// <param name="models">Кортеж сущностей: ZL_LIST_ENTITY (основные данные счёта, H-формат) и PERS_LIST_ENTITY (персональные данные, L-формат)</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task RewriteInvoiceAsync(
        int schetUid,
        (ZlListEntity, PersListEntity) models,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Выполняет логический контроль счета
    /// </summary>
    /// <param name="schetUid">Uid счета у которого нужно провести логический контроль</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken ">Токен отмены</param>
    /// <returns></returns>
    Task PerformLogicControlAsync(
        int schetUid,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Проверить существование счёта в БД путём вызова хранимой процедуры.
    /// </summary>
    /// <param name="request">Данные для проверки счёта: сущности и имена файлов.</param>
    /// <param name="journalType">Тип журнала, в котором выполняется проверка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>DTO с параметрами результата проверки счёта.</returns>
    Task<InvoiceCheckOutput> CheckInvoiceExistenceAsync(
        InvoiceCheckParameters request,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);
}
