using HealthInvoice.Core.Dtos.Auth;

namespace HealthInvoice.Core.Interfaces.Services.Authorization;

/// <summary>
/// Сервис для выполнения авторизации/деавторизации пользователя.
/// Содержит методы авторизации пользователя по имени и поролю, а так же деавторизацию.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Производит авторизацию пользователя по имени и паролю.
    /// Выполняет сравнение введённого пароля с хешированным значением в БД.
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>
    /// Объект AuthenticationResult, содержащий:
    /// - bool IsSuccess: true, если авторизация пройдена.
    /// - string ErrorMessage: описание ошибки (если IsSuccess = false).
    /// </returns>
    Task<LoginResponse> LoginAsync(
        string username, 
        string password,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Выполняет деавторизацию пользователя — очищает контекст сессии,
    /// аннулирует токены доступа (если используются) и обновляет статус в системе.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>true, если деавторизация выполнена успешно; false — в случае ошибки.</returns>
    Task<bool> LogoutAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет регистрацию пользователя в системе.
    /// </summary>
    /// <param name="request">Модель запроса, содержащая данные регистрируемых пользователей</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    Task RegisterUserAsync(
        RegisterUsersRequest request,
        CancellationToken cancellationToken = default);
}
