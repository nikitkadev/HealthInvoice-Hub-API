namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>
/// Информация о пользователе для ответа на фронт.
/// </summary>
/// <param name="Uid">Уникальный идентификатор пользователя.</param>
/// <param name="Username">Имя пользователя (логин).</param>
/// <param name="Surname">Фамилия.</param>
/// <param name="Name">Имя.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Phone">Контактный телефон.</param>
/// <param name="OrganizationCode">Код организации.</param>
/// <param name="OrganizationName">Название организации.</param>
/// <param name="SessionStart">Начало сессии.</param>
/// <param name="SessionEnd">Окончание сессии (если есть).</param>
/// <param name="LastActivity">Время последней активности.</param>
/// <param name="IsAcceptedPersonalData">Согласие на обработку ПД.</param>
public record UserInformationResponse(
    int Uid,
    string Username,
    string Surname,
    string Name,
    string Patronymic,
    string Phone,
    string OrganizationCode,
    string OrganizationName,
    string SessionStart,
    string? SessionEnd,
    DateTime LastActivity,
    bool IsAcceptedPersonalData
);
