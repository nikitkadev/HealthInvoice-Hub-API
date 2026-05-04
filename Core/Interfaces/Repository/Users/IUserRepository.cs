using HealthInvoice.Core.Entities.Domain;

namespace HealthInvoice.Core.Interfaces.Repository.Users;

/// <summary>
/// Репозиторий для выполнения операций чтения/записи данных о пользователях.
/// Предоставляет методы для получения сущностей User по различным критериям.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получить сущность пользователя по имени пользователя (логину).
    /// </summary>
    /// <param name="username">Имя пользователя (логин), для которого требуется получить данные.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>
    /// Сущность User, если пользователь найден в БД.
    /// null, если пользователь с указанным именем не существует.
    /// </returns>
    Task<User?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Создает запись в таблице User в базе данных.
    /// </summary>
    /// <param name="users">Сущности User, которые будут записаны в таблицу.</param>
    Task AddUsersAsync(List<User> users);

    /// <summary>
    /// Удаляет запись из таблицы Users под первичному ключу.
    /// </summary>
    /// <param name="userUid">Значение первичного ключа строки.</param>
    Task RemoveUserAsync(int userUid);

    /// <summary>
    /// Возвращает все записи из таблицы User.
    /// </summary>
    /// <returns>Список сущностей User</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Обновляет время последней активности у пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя, у которого нужно обновить время последней активности.</param>
    Task UpdateActivityTimeAsync(string username);

    /// <summary>
    /// Меняет флаг принятия обработки персональных данных для пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя. принявшего обработку персональных данных.</param>
    Task AcceptPersonalDataProcessingAsync(string username);

    /// <summary>
    /// Получает имя организации пользователя
    /// </summary>
    /// <param name="organizationCode"></param>
    /// <returns>Строка организация пользователя.</returns>
    Task<string> GetUserOrganizationName(string organizationCode);
}