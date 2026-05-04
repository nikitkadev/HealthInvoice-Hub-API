using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Files;

namespace HealthInvoice.Core.Interfaces.Repository.Helpers;

/// <summary>
/// Дополнительный сервис для специфичных запросов к базе данных.
/// </summary>
public interface IRepositoryHelper
{
    /// <summary>
    /// Метод для получения имен для файлов и тегов внутри отчета МЭК.
    /// </summary>
    /// <param name="schetUid">Uid счета по которому создается отчет.</param>
    /// <param name="journalType">Тип журнала, в котором лежит проверенный счет.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Модель, содержащая в себе имена файлов и тегов для отчета МЭК.</returns>
    public Task<LogicControlFileNamesResult> GetFileNamesForControlReportAsync(
        int schetUid,
        JournalType journalType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод для получения имен для файлов и тегов внутри отчета форматной проверки.
    /// </summary>
    /// <param name="sourceFilename">Имя архива, для которого проводился форматный контроль.</param>
    /// <param name="journalType">Тип выбранного журнала.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Модель, содержащая в себе имена файлов и тегов для отчета форматного контроля.</returns>
    public Task<FormatControlFileNamesResult> GetFileNamesForFormatReportAsync(
        string sourceFilename,
        JournalType journalType,
        CancellationToken cancellationToken = default);
}
