
namespace HealthInvoice.Core.Dtos.Auth;

/// <summary>
/// Модель-запрос с данными о новом пользователе.
/// </summary>
/// <param name="Username">Имя пользователя</param>
/// <param name="Password">Пароль</param>
/// <param name="OrganizationCode">Код орагнизации</param>
/// <param name="Name">Имя</param>
/// <param name="Surname">Фамилия</param>
/// <param name="Patronymic">Отчество</param>
/// <param name="PhoneNumber">Контактный телефон</param>

public record RegisterUsersRequest(
    string Username,
    string Password,
    string OrganizationCode,
    string Name,
    string Surname,
    string Patronymic,
    string PhoneNumber);