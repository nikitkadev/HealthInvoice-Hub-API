using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Files;

namespace HealthInvoice.Core.Interfaces.Services.Invoices.Exporter;

/// <summary>
/// Сервис для создания отчётов проверки отправленных счетов.
/// Предоставляет методы для генерации отчётов о форматной и логической проверке счетов.
/// </summary>
public interface IReportExporter
{
    /// <summary>
    /// Создаёт отчёт по форматной проверке счёта.
    /// </summary>
    /// <param name="dto">
    /// Объект InvoiceFormatValidationResultDto, содержащий результаты форматной проверки счёта.
    /// Включает информацию об ошибках формата, структуре файла и соответствии стандартам.
    /// </param>
    /// <param name="organizationCode">
    /// Код организации, для которой формируется отчёт (используется для идентификации
    /// и группировки отчётов по организациям).
    /// </param>
    /// <returns>
    /// MemoryStream — поток данных созданного отчёта в формате файла.
    /// Поток позиционирован в начало (Position = 0) и готов к использованию.
    /// </returns>
    Task<MemoryStream> CreateFormatValidationReportAsync(
        string sourceArchiveFilename,
        FormatControlFileNamesResult namingDto,
        JournalType journalType,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Создаёт отчёт по логической проверке счёта.
    /// </summary>
    /// <param name="schetUid">
    /// Уникальный идентификатор (UID) счёта, по которому выполнялась медико‑экономическая
    /// экспертиза (МЭК). Используется для получения данных о счёте из базы данных.
    /// </param>
    /// <param name="namingDto">
    /// Модель, содержащая в себе имена файлов и тегов, которые необходимы для создания отчета.
    /// </param>
    /// <returns>
    /// MemoryStream — поток данных созданного отчёта.
    /// Содержит полный контент отчёта в выбранном формате (PDF, XLSX и т. д.).
    /// Поток позиционирован в начало и готов к передаче/сохранению.
    /// </returns>
    Task<MemoryStream> CreateLogicalValidationReportAsync(
        int schetUid,
        LogicControlFileNamesResult namingDto,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);
}
