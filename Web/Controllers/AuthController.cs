using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Interfaces.Services.Authorization;
using HealthInvoice.Core.Dtos.Auth;
using HealthInvoice.Core.Common;

namespace HealthInvoice.Web.Controllers;

[ApiController]
[Route("healthinvoice/api/auth")]
public class AuthController(
    IAuthorizationService authorizationService,
    IUserRepository userRepository,
    IUsersDeserializer usersDeserializer) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        try
        {
            var loginResult = await authorizationService.LoginAsync(
            loginRequest.Username,
            loginRequest.Password,
            cancellationToken);

            if (!loginResult.IsSuccess)
                return Unauthorized();

            HttpContext.Session.SetString("Username", loginResult.Username);
            HttpContext.Session.SetString("Surname", loginResult.Surname);
            HttpContext.Session.SetString("Name", loginResult.Name);
            HttpContext.Session.SetString("Patronymic", loginResult.Patronymic);
            HttpContext.Session.SetString("Phone", loginResult.Phone);
            HttpContext.Session.SetString("OrganizationCode", loginResult.OrganizationCode);
            HttpContext.Session.SetString("OrganizationName", loginResult.OrganizationName);
            HttpContext.Session.SetString("SessionStart", loginResult.SessionStart ?? string.Empty);
            HttpContext.Session.SetString("PersonalDataAccepted", loginResult.IsAcceptedPersonalData.ToString());

            return Ok();
        }
        catch (UserIsNotFoundException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetSessionInformationAsync()
    {
        var username = HttpContext.Session.GetString("Username");
        var surname = HttpContext.Session.GetString("Surname");
        var name = HttpContext.Session.GetString("Name");
        var patronymic = HttpContext.Session.GetString("Patronymic");
        var phone = HttpContext.Session.GetString("Phone");
        var organizationCode = HttpContext.Session.GetString("OrganizationCode");
        var organizationName = HttpContext.Session.GetString("OrganizationName");
        var sessionStart = HttpContext.Session.GetString("SessionStart");
        var isAcceptedPersonalData = HttpContext.Session.GetString("PersonalDataAccepted");

        if (string.IsNullOrWhiteSpace(username))
            return Unauthorized();

        return Ok(
            new UserInformationResponse(
                Uid: 0,
                Username: username,
                Surname: surname ?? string.Empty,
                Name: name ?? string.Empty,
                Patronymic: patronymic ?? string.Empty,
                Phone: phone ?? string.Empty,
                OrganizationCode: organizationCode ?? string.Empty,
                OrganizationName: organizationName ?? string.Empty,
                SessionStart: sessionStart ?? string.Empty,
                SessionEnd: sessionStart,
                LastActivity: DateTime.Now,
                IsAcceptedPersonalData: bool.Parse(isAcceptedPersonalData ?? string.Empty)
                ));
    }

    [HttpPost("bulk-register")]
    public async Task<IActionResult> RegistreUserAsync(IFormFile usersData, CancellationToken cancellationToken)
    {
        try
        {
            var users = usersDeserializer.DeserializeUsers(
                usersData.OpenReadStream());

            await authorizationService.RegisterUserAsync(
                new RegisterUsersRequest(Users: users), 
                cancellationToken);
            
            return Ok();
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(
                new
                {
                    ex.Message
                });
        }

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