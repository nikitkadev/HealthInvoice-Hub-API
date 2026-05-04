using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Interfaces.Managers;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Interfaces.Repository.Invoices;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/invoices")]
public class InvoicesController(
    IQueuePublisher<InvoiceDbOperationMessage> dbOperationPublisher,
    IQueuePublisher<InvoiceLogicControlMessage> logicControllingPublisher,
    ILkJournalRepository journalRepository,
    IInvoiceRepository invoiceRepository,
    IInvoiceManager invoiceManager) : ControllerBase
{
    [HttpPost("format-control")]
    [RequestSizeLimit(FileConstants.FilesSizeLimit)]
    public async Task<IActionResult> ValidateInvoiceFormatAsync(
        IFormFile[] files, 
        [FromForm] string journalType, 
        CancellationToken cancellationToken)
    {
        var codeMo = HttpContext.Session.GetString("OrganizationCode");

        if (string.IsNullOrWhiteSpace(codeMo))
        {
            return Unauthorized();
        }

        if (!int.TryParse(journalType, out int type))
        {
            return BadRequest();
        }

        var invoices = files
            .Select(
                file => new InvoiceArchive()
                {
                    ArchiveStream = file.OpenReadStream(),
                    ArchiveFilename = file.FileName,
                    OrganizationCode = codeMo,
                })
            .ToList();

        var formatControlResult = await invoiceManager.ValidateInvoiceStructureAsync(
            new InvoiceValidationParameters()
            {
                Archives = invoices,
                JournalType = (JournalType)type
            },
            cancellationToken);

        return Ok(formatControlResult);
    }

    [HttpPost("logic-control")]
    public async Task<IActionResult> ValidateInvoiceLogicalAsync([FromBody] SelectedInvoiceRequest request, CancellationToken cancellationToken)
    {
        if(request is null || request.SchetUids.Count == 0)
            return BadRequest();

        await journalRepository.UpdateInvoiceStatusAsync(
            request.SchetUids, 4, (JournalType)request.JournalType, cancellationToken);

        var toSendLogicControl = request.SchetUids.Select(schetUid => new InvoiceLogicControlMessage(
                SchetUid: schetUid,
                JournalType: (JournalType)request.JournalType)).ToList();

        await logicControllingPublisher.PublishAsync(
            toSendLogicControl,
            cancellationToken: cancellationToken);

        return Accepted();
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertInvoicesAsync([FromBody] InvoicesUpsertRequest request, CancellationToken cancellationToken)
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrWhiteSpace(username))
            return Unauthorized(
                "Не удалось получить данные по сессии. Пожалуйста, авторизуйтесь повторно.");

        if (request.Items.Count == 0)
            return BadRequest();

        await dbOperationPublisher.PublishAsync(
            [.. request.Items.Select(
                item => new InvoiceDbOperationMessage(
                    FileName: item.Filename,
                    FilePath: item.FilePath,
                    Uploader: username,
                    SchetUid: item.SchetUid,
                    JournalType: (JournalType)request.JournalType,
                    DbOperation: item.SchetUid is null ? DbOperation.Insert : DbOperation.Update)
                )
            ],
            cancellationToken: cancellationToken);

        return Accepted();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> DeleteInvoicesAsync([FromBody] SelectedInvoiceRequest request, CancellationToken cancellationToken)
    {
        if (request is null || request.SchetUids.Count == 0)
            return BadRequest();

        foreach (var item in request.SchetUids)
            await invoiceRepository.RemoveInvoiceAsync(
                item, 
                (JournalType)request.JournalType, 
                cancellationToken);

        return Ok();
    }
}