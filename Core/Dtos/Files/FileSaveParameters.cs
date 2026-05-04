namespace HealthInvoice.Core.Dtos.Files;

/// <summary>Параметры для сохранения файла на диск.</summary>
public class FileSaveParameters
{
    /// <summary>Поток данных файла.</summary>
    public required Stream FileStream { get; set; }

    /// <summary>Исходное имя файла.</summary>
    public required string FileName { get; set; }

    /// <summary>Путь для сохранения.</summary>
    public required string SavePath { get; set; }

    /// <summary>Код организации.</summary>
    public required string OrganizationCode { get; set; }
}