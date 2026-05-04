namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>
/// Результат авторизации пользователя.
/// </summary>
/// <param name="IsSuccess">Успешность авторизации.</param>
/// <param name="ErrorMessage">Сообщение об ошибке (если есть).</param>
/// <param name="Username">Имя пользователя.</param>
/// <param name="Surname">Фамилия.</param>
/// <param name="Name">Имя.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Phone">Телефон.</param>
/// <param name="OrganizationName">Название организации.</param>
/// <param name="OrganizationCode">Код организации.</param>
/// <param name="IsAcceptedPersonalData">Согласие на обработку ПД.</param>
public record LoginResponse(
    bool IsSuccess,
    string? ErrorMessage,
    string Username,
    string Surname,
    string Name,
    string Patronymic,
    string Phone,
    string OrganizationName,
    string OrganizationCode,
    bool IsAcceptedPersonalData
)
{
    /// <summary>Начало сессии (только при успешной авторизации).</summary>
    public string? SessionStart => IsSuccess ? DateTime.UtcNow.ToString("g") : null;
}