namespace HealthInvoice.Core.Interfaces.Services.Files;

public interface IFilePathService
{
    string CreateFilePath(string filename, string root, params string[] subFolders);
}
