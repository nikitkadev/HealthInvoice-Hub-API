using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Interfaces.Repository.Helpers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Exporter;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/report")]
public class ReportController(
    IReportExporter reportExporter,
    IRepositoryHelper helper) : ControllerBase
{
    [HttpGet("download-format")]
    public async Task<IActionResult> DownloadFormatValidationResultReportAsync(
        string sourceArchiveFilename, 
        int journalType, 
        CancellationToken cancellationToken)
    {
        try
        {
            var filesNaming = await helper.GetFileNamesForFormatReportAsync(
                sourceArchiveFilename, 
                (JournalType)journalType, 
                cancellationToken);

            var reportStream = await reportExporter.CreateFormatValidationReportAsync(
                sourceArchiveFilename,
                filesNaming,
                (JournalType)journalType,
                cancellationToken);
            
            return File(reportStream, "application/zip", filesNaming.ArchiveFilename);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка чтения файла: {ex.Message}");
        }
    }

    [HttpGet("download-control")]
    public async Task<IActionResult> DownloadControlValidationResultReportAsync(
        int schetUid, 
        int journalType, 
        CancellationToken cancellationToken)
    {
        try
        {
            var filesNaming = await helper.GetFileNamesForControlReportAsync(
                schetUid, 
                (JournalType)journalType, 
                cancellationToken);

            var reportStream = await reportExporter.CreateLogicalValidationReportAsync(
                schetUid, 
                filesNaming, 
                (JournalType)journalType, 
                cancellationToken);

            return File(reportStream, "application/zip", filesNaming.ArchiveFilename);
        }
        catch (Exception)
        {
            return StatusCode(500, $"Бубу");
        }
    }
}
