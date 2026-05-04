namespace HealthInvoice.Core.Dtos.Files;

/// <summary>Результат получения имён файлов для логического контроля (МЭК).</summary>
public record LogicControlFileNamesResult
{
    /// <summary>Имя ZIP-архива.</summary>
    public string ArchiveFilename { get; init; } = string.Empty;

    /// <summary>Имя XML-файла (H или L).</summary>
    public string XmlFilename { get; init; } = string.Empty;

    /// <summary>Имя тега.</summary>
    public string TagFilename { get; init; } = string.Empty;

    /// <summary>Дополнительное имя тега (для L-файла).</summary>
    public string AdditionalTagFilename { get; init; } = string.Empty;
}