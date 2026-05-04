namespace HealthInvoice.Core.Dtos.Files;


/// <summary>Результат получения имён файлов для форматного контроля.</summary>
public record FormatControlFileNamesResult
{
    /// <summary>Имя ZIP-архива.</summary>
    public string ArchiveFilename { get; init; } = string.Empty;

    /// <summary>Оригинальное имя H-файла.</summary>
    public string HOriginalFilename { get; init; } = string.Empty;

    /// <summary>Имя тега H-файла.</summary>
    public string HTagFilename { get; init; } = string.Empty;

    /// <summary>Callback имя H-файла.</summary>
    public string HCallbackFilename { get; init; } = string.Empty;

    /// <summary>Оригинальное имя L-файла.</summary>
    public string LOriginalFilename { get; init; } = string.Empty;

    /// <summary>Имя тега L-файла.</summary>
    public string LTagFilename { get; init; } = string.Empty;

    /// <summary>Callback имя L-файла.</summary>
    public string LCallbackFilename { get; init; } = string.Empty;
}