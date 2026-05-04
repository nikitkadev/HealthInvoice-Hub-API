using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Auth;
using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Interfaces.Services.Authorization;
using HealthInvoice.Core.Entities.Domain;

namespace HealthInvoice.Infrastructure.Implementation.Services.Authorization;

public class AuthorizationService(
    ILogger<AuthorizationService> logger,
    IPasswordHasherService passwordHasherService,
    IUserRepository userRepository) : IAuthorizationService
{
    public async Task<LoginResponse> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userRepository
                .GetUserByUsernameAsync(username, cancellationToken)
                ?? throw new UserIsNotFoundException(username);

            if (!passwordHasherService.VerifyPassword(password, user.PasswordHash))
                return new LoginResponse(
                    IsSuccess: false,
                    ErrorMessage: "Введен неверный пароль",
                    Username: username,
                    Surname: user.Surname,
                    Name: user.Name,
                    Patronymic: user.Patronymic,
                    Phone: user.Phone,
                    OrganizationName: user.OrganiztionName,
                    OrganizationCode: user.CodeOrg,
                    IsAcceptedPersonalData: user.PersDataAccepted);

            return new LoginResponse(
                IsSuccess: true,
                ErrorMessage: string.Empty,
                Username: username,
                Surname: user.Surname,
                Name: user.Name,
                Patronymic: user.Patronymic,
                Phone: user.Phone,
                OrganizationName: await userRepository.GetUserOrganizationName(user.CodeOrg),
                OrganizationCode: user.CodeOrg,
                IsAcceptedPersonalData: user.PersDataAccepted); 
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(
                ex,
                "Операция проверки пользователя {Username} при входе в учетную запись прервана токеном",
                username);

            throw;
        }
    }

    public async Task RegisterUserAsync(
        RegisterUsersRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.IsEmpty)
                return;

            var users = new List<User>();

            foreach (var user in request.Users)
            {
                var organizationName = await userRepository.GetUserOrganizationName(user.OrganizationCode);

                users.Add(
                    new User
                    {
                        Username = user.Username,
                        PasswordHash = passwordHasherService.HashPassword(user.Password),
                        CodeOrg = user.OrganizationCode,
                        OrganiztionName = organizationName,
                        Name = user.Name,
                        Surname = user.Surname,
                        Patronymic = user.Patronymic,
                        PersDataAccepted = false,
                        Phone = user.PhoneNumber,
                        LastActivity = DateTime.Now.AddDays(-1)
                    });
            }

            await userRepository.AddUsersAsync(users);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке добавить нового пользователя");

            throw;
        }
    }
    public Task<bool> LogoutAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
