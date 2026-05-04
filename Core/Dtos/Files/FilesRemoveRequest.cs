namespace HealthInvoice.Core.Dtos.Files;

/// <summary>
/// Запрос на удаление файлов.
/// </summary>
/// <param name="FilesToRemove">Список файлов для удаления.</param>
public record FilesRemoveRequest(
    List<FileMetaDto> FilesToRemove
)
{
    public bool IsValid => FilesToRemove is not null && FilesToRemove.Count > 0;
}

/// <summary>
/// Метаданные файла.
/// </summary>
/// <param name="Filename">Имя файла.</param>
/// <param name="FilePath">Полный путь к файлу.</param>
public record FileMetaDto(
    string Filename,
    string FilePath
);