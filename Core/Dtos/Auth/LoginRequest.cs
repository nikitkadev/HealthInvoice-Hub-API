namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>
/// Транспортный класс, содержащий введенный пользователем логин и пароль.
/// </summary>
/// <param name="Username">Имя пользователя.</param>
/// <param name="Password">Пароль (в открытом виде, без хэширования).</param>
public record LoginRequest(
    string Username,
    string Password)
{
    /// <summary>Проверка валидности отправленных данных.</summary>
    public bool IsValid => 
        !string.IsNullOrWhiteSpace(Username) && 
        !string.IsNullOrWhiteSpace(Password);
}
