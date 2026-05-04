using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Invoices;

namespace HealthInvoice.Core.Interfaces.Services.Invoices.Validators;

public interface IInvoiceValidator
{
    /// <summary>
    /// Проверяет мета-информацию отправленного ZIP-архива
    /// </summary>
    /// <param name="fileStream">Отправленный файл в виде потока</param>
    /// <param name="fileName">Имя отправленного файла</param>
    /// <returns>Результат проверки входящего ZIP-файла</returns>
    Task<IntegrityValidationResult> ValidateFileMetadata(
        Stream fileStream,
        string uploadFilename,
        JournalType journalType,
        string organizationCode = OrganizationConstants.AdminOrgCode);

    /// <summary>
    /// Проверяет формат отправленных счетов
    /// </summary>
    /// <param name="archiveStream">Отправленный файл в виде потока</param>
    /// <param name="archiveFilename">Имя отправленного файла</param>
    /// <returns>Результат полной проверки формата входных счетов</returns>
    Task<ComplianceValidationResult> ValidateInvoiceFormatAsync(
        Stream archiveStream, 
        string archiveFilename,
        JournalType journalType);
}
