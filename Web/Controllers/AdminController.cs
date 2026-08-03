using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Auth;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/admin")]
public class AdminController(IUserRepository userRepository) : ControllerBase
{

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
