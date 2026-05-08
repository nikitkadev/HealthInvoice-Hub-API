namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>Результат авторизации пользователя.</summary>
/// <param name="IsSuccess">Успешность авторизации.</param>
/// <param name="IsAcceptedPersonalPolicy">Флаг согласия на обработку персональных данных.</param>
/// <param name="Username">Имя пользователя.</param>
/// <param name="OrganizationCode">Код организации.</param>
/// <param name="ClientMessage">Сообщение клиенту.</param>
public record LoginResult(
    bool IsSuccess,
    bool IsAcceptedPersonalPolicy,
    string Username,
    string OrganizationCode,
    string ClientMessage
);