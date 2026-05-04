using HealthInvoice.Core.Interfaces.Services.Invoices.Management;

namespace HealthInvoice.Infrastructure.Implementation.Services.Invoices.Management;

public class FileSchemaSource : ISchemaSource
{
    public Stream OpenSchemaStream(
        string schemaName, 
        CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Schemas", schemaName);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Схема не найдена: {path}");

        return File.Open(path, FileMode.Open);
    }
}
