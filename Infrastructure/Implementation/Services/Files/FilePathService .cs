using HealthInvoice.Core.Interfaces.Services.Files;

namespace HealthInvoice.Infrastructure.Implementation.Services.Files;

public class FilePathService : IFilePathService
{
    public string CreateFilePath(string filename, string root, params string[] subFolders)
    {
        string currentPath = root;

        foreach(var sub in subFolders)
            currentPath  = Path.Combine(currentPath, sub);

        return Path.Combine(currentPath, filename);
    }
}
