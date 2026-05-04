using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Interfaces.Repository.Rcontrol;
using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Rcontrol.Tables;
using HealthInvoice.Core.Dtos.Auth;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/admin")]
public class AdminController(
    IRControlViewSummaryData rControlViewSummaryData,
    IUserRepository userRepository) : ControllerBase
{
    [HttpGet("rcontrol/medorg")]
    public async Task<IActionResult> GetMedOrgAsync(int journalType)
    {
        var result = await rControlViewSummaryData.GetMedOrgsAsync((JournalType)journalType);

        return Ok(result);
    }

    [HttpGet("rcontrol/periods")]
    public async Task<IActionResult> GetMedOrgAsync(string codeMo, int journalType)
    {
        var result = await rControlViewSummaryData.GetPeriodsAsync(codeMo, (JournalType)journalType);

        return Ok(result);
    }

    [HttpGet("rcontrol/invoices_shortly")]
    public async Task<IActionResult> GetRecordsForTable_3Async(
        string codeMo, 
        int Year, 
        byte Month, 
        int journalType, 
        CancellationToken cancellationToken)
    {
        var result = await rControlViewSummaryData.GetInvoicesShortlyRecordsAsync(
            codeMo, 
            Year, 
            Month, 
            (JournalType)journalType, 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("rcontrol/invoice_summary")]
    public async Task<IActionResult> GetSummaryInvoiceAsync(int schetUid, int journalType)
    {
        var result = await rControlViewSummaryData.GetInvoiceSummaryAsync(schetUid, (JournalType)journalType);
        return Ok(result);
    }

    [HttpGet("rcontrol/finished_cases")]
    public async Task<IActionResult> GetFinishedCasesAsync(int schetUid, int journalType)
    {
        var result = await rControlViewSummaryData.GetFinishedCasesAsync(schetUid, (JournalType)journalType);
        return Ok(result);
    }

    [HttpGet("rcontrol/cases")]
    public async Task<IActionResult> GetCasesAsync(int zSlUid, int journalType)
    {
        var result = await rControlViewSummaryData.GetCasesAsync(zSlUid, (JournalType)journalType);
        var response = result.Select(
            result => new CaseDto()
            {
                Profil = result.Profil,
                Det = result.Det,
                Prvs = result.Prvs,
                StartingAt = result.Date1,
                EndingAt = result.Date2,
                Ds1 = result.Ds1,
                EdCol = result.EdCol,
                Tarif = result.Tarif,
                SumM = result.SumM,
                Sump = 0,
                SmoSump = result.SmoSump
            }).ToList();

        return Ok(response);
    }

    [HttpGet("users/get")]
    public async Task<IActionResult> GetAppUsersAsync()
    {
        var dbUsers = await userRepository.GetAllUsersAsync();

        var response = dbUsers.Select(
            dbUser => new UserInformationResponse(
                Uid: dbUser.Uid,
                Username: dbUser.Username,
                Surname: dbUser.Surname,
                Name: dbUser.Name,
                Patronymic: dbUser.Patronymic,
                Phone: dbUser.Phone,
                OrganizationCode: dbUser.CodeOrg,
                OrganizationName: dbUser.OrganiztionName,
                SessionStart: DateTimeOffset.Now.ToString("g"),
                SessionEnd: DateTimeOffset.Now.AddHours(9).ToString("g"),
                LastActivity: dbUser.LastActivity,
                IsAcceptedPersonalData: false)).ToList();
            

        return Ok(response);
    }

    [HttpPost("users/remove")]
    public async Task<IActionResult> RemoveUserAsync([FromBody] int userUid)
    {
        try
        {
            await userRepository.RemoveUserAsync(userUid);
            return Ok();

        }
        catch (UserIsNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
