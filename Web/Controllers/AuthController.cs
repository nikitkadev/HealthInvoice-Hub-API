using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Dtos.Auth;
using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Interfaces.Services.Authorization;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/auth")]
public class AuthController(
    IAuthorizationService authorizationService,
    IUserRepository userRepository) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var loginResult = await authorizationService.LoginAsync(
            loginRequest.Username,
            loginRequest.Password,
            cancellationToken);

        HttpContext.Session.SetString("Username", loginResult.Username);
        HttpContext.Session.SetString("OrganizationCode", loginResult.OrganizationCode);

        return Ok(loginResult);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    [HttpPost("registe")]
    public async Task<IActionResult> RegisteUserAsync(
        [FromBody] RegisterUsersRequest request,
        CancellationToken cancellationToken)
    {
        if(request is null)
        {
            return BadRequest();
        }

        await authorizationService.RegisteUserAsync(
            request: request,
            cancellationToken: cancellationToken);

        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var dbUser = await userRepository.GetUserByUsernameAsync(username);

        if (dbUser is null)
        {
            return NotFound();
        }

        return Ok(new UserInformationResponse(
            Uid: dbUser.Uid,
            Username: dbUser.Username,
            Surname: dbUser.Surname,
            Name: dbUser.Name,
            Patronymic: dbUser.Patronymic,
            Phone: dbUser.Phone,
            OrganizationCode: dbUser.CodeOrg,
            OrganizationName: dbUser.OrganiztionName,
            SessionStart: DateTime.Now.ToString("g"),
            SessionEnd: DateTime.Now.AddHours(8).ToString("g"),
            LastActivity: dbUser.LastActivity,
            IsAcceptedPersonalData: dbUser.PersDataAccepted));
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> UpdateHeartbeat()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        await userRepository.UpdateActivityTimeAsync(username);

        return Ok();
    }

    [HttpPost("accept_pers")]
    public async Task<IActionResult> AcceptConsentAsync()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        await userRepository.AcceptPersonalDataProcessingAsync(username);

        return Ok();
    }
}