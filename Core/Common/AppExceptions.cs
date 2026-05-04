namespace HealthInvoice.Core.Common;

/// <summary>Базовый класс для всех доменных исключений.</summary>
public abstract class AppException(string errorMessage) : Exception(errorMessage);

/// <summary>Пользователь уже существует</summary>
public sealed class UserAlreadyExistsException(string username) 
    : AppException($"Пользователь `{username}` уже существует")
{
    public string Username { get; } = username;
}

/// <summary>Пользователь не найден</summary>
public sealed class UserIsNotFoundException(string username) 
    : AppException($"Не удалось найти пользователя: {username}")
{
    public string Username { get; set; } = username;
}

/// <summary>Счет в журнале форматного контроля не найден</summary>
/// <param name="sourceArchiveFilename"></param>
public sealed class InvoiceInFormatControlJournalNotFound(string sourceArchiveFilename)
    : AppException($"Архив {sourceArchiveFilename} отсутствует в журнале форматного контроля")
{
    public string SourceArchiveFilename { get; set; } = sourceArchiveFilename;
}