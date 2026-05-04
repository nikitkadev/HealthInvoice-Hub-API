using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Reports;
using HealthInvoice.Core.Dtos.Database.QueryResults;
using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Core.Interfaces.Repository.Reports;

/// <summary>
/// Репозиторий для выполнения запросов к данным об ошибках проверки МЭК.
/// Предоставляет методы для получения сводных данных о дефектах, связанных со счетами.
/// </summary>
public interface IDefectsSummaryQueryRepository
{
    /// <summary>
    /// Получить список ошибок проверки МЭК для указанного счёта.
    /// </summary>
    /// <param name="schetUid">Идентификатор счёта, для которого требуется получить ошибки проверки МЭК.</param>
    /// <param name="journalType">Тип выбранного журнала</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список DTO ошибок проверки МЭК, связанных с указанным счётом.
    Task<List<LogicControlDefectDto>> GetLkDefectsAsync(
        int schetUid,
        JournalType journalType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список ошибок проверки форматного контроля.
    /// </summary>
    /// <param name="sourceArchiveFilename">Имя проверенного файла.</param>
    /// <param name="journalType">Тип журнала.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список DTO ошибок проверки форматного контроля</returns>
    Task<List<FormatControlDefectEntity>> GetFkDefectsAsync(
        string sourceArchiveFilename,
        JournalType journalType,
        CancellationToken cancellationToken = default);
}
