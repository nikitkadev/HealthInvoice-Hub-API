using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Dtos.Files;
using HealthInvoice.Core.Interfaces.Services.Files;

namespace HealthInvoice.Infrastructure.Implementation.Services.Files;

public class LocalStorageService(
    ILogger<LocalStorageService> logger) : IFileStorageService
{
    public async Task StoreFileAsync(
        FileSaveParameters fileData, 
        CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(fileData);

            Directory.CreateDirectory(
                Path.GetDirectoryName(fileData.SavePath) ?? string.Empty);

            await using var fileStream = new FileStream(fileData.SavePath, FileMode.Create);

            await fileData.FileStream.CopyToAsync(fileStream, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(
                ex,
                "Переданный аргумент filePath является null");

            throw;
        }
        catch (IOException ex)
        {
            logger.LogError(
                ex, 
                "Ошибка записи файла: {FilePath}", 
                fileData?.SavePath);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Неожиданная ошибка при сохранении файла: {FilePath}", 
                fileData?.SavePath);

            throw;
        }
    }

    public async Task<Stream> RetrieveFileStreamAsync(
        string filePath, 
        CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(filePath);

            var ms = new MemoryStream();
            await using var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            await file.CopyToAsync(ms, cancellationToken);
            ms.Position = 0;

            return ms;
        }
        catch(ArgumentNullException ex)
        {
            logger.LogError(
                ex,
                "Переданный аргумент filePath является null");

            throw;
        }
        catch (IOException ex)
        {
            logger.LogError(
                ex,
                "Ошибка I/O операции при попытке получить хранимый файл {FilePath}",
                filePath);

            throw;
        }
    }

    public Task RemoveFileAsync(string filePath)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(filePath);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(
                ex,
                "Переданный аргумент filePath является null");

            throw;
        }
    }

    public void RemoveFilesFromDirectoryAsync(string path)
    {
        var rootDir = new DirectoryInfo(path);

        foreach(var file in rootDir.GetFiles())
            file.Delete();

        foreach(var dir in rootDir.GetDirectories())
            dir.Delete(true);
    }
}
