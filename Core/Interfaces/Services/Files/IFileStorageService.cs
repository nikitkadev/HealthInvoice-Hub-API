using HealthInvoice.Core.Dtos.Files;

namespace HealthInvoice.Core.Interfaces.Services.Files;

/// <summary>
/// Сервис для работы с файловым хранилищем: загрузка, получение и удаление файлов.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Загружает файл в хранилище.
    /// </summary>
    /// <param name="fileData">Данные для сохранения файла.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task StoreFileAsync(
        FileSaveParameters fileData,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Получает поток данных файла из хранилища.
    /// </summary>
    /// <param name="filePath">Полный путь к файлу в хранилище.</param>
    /// <returns>Поток данных запрашиваемого файла.</returns>
    Task<Stream> RetrieveFileStreamAsync(
        string filePath,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Удаляет файл из хранилища.
    /// </summary>
    /// <param name="filePath">Полный путь к удаляемому файлу.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    Task RemoveFileAsync(string filePath); 

    /// <summary>
    /// Чистит указанную директорию от всех файлов и папок.
    /// </summary>
    /// <param name="directoryPath">Путь к директории.</param>
    void RemoveFilesFromDirectoryAsync(string directoryPath);
}
