using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Dtos.Invoices;

namespace HealthInvoice.Core.Interfaces.Managers;

/// <summary>
/// Менеджер для обработки бизнес‑операций со счетами: регистрация, обновление и валидация.
/// </summary>
public interface IInvoiceManager
{
    /// <summary>
    /// Регистрирует новый счёт в системе.
    /// </summary>
    /// <param name="dto">Данные нового счёта для регистрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию регистрации.</returns>
    Task RegisterInvoiceAsync(
        string filePath,
        string uploader,
        JournalType journalType,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Обновляет запись о счёте в системе.
    /// </summary>
    /// <param name="dto">Данные для обновления записи о счёте.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    Task UpdateInvoiceAsync(
        string filePath,
        string uploader,
        int schetUid,
        JournalType journalType,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Выполняет полную форматную проверку счетов.
    /// </summary>
    /// <param name="invoices">
    /// Коллекция InvoiceFileRequest, содержащая следующие поля:
    /// - ArchiveStream: Содержимое файла в виде потока.
    /// - Filename: Имя отправленного файла.
    /// - OrganizationCode: Код организации, которая отправила файл.
    /// - JournalType: Тип выбранного журнала.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Коллекция InvoiceFormatControlResponse объектов с общим результатом проверок файлов.</returns>
    Task<List<InvoiceFormatControlResponse>> ValidateInvoiceStructureAsync(
        InvoiceValidationParameters invoices,
        CancellationToken cancellationToken = default);
}