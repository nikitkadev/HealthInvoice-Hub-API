
namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>
/// Запрос на регистрацию новых пользователей.
/// </summary>
/// <param name="Users">Имя пользователя (логин).</param>

public record RegisterUsersRequest(
    List<UserJsonConvertDto> Users)
{
    public bool IsEmpty => Users.Count == 0;
}