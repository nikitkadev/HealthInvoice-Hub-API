using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Параметры для проверки счёта в базе данных.
/// </summary>
public class InvoiceCheckParameters
{
    /// <summary>Сущность H-счёта (после парсинга XML).</summary>
    public required ZlListEntity HInvoiceEntity { get; set; }

    /// <summary>Сущность L-счёта (после парсинга XML).</summary>
    public required PersListEntity LInvoiceEntity { get; set; }

    /// <summary>Имя H-файла.</summary>
    public required string HInvoiceFilename { get; set; }

    /// <summary>Имя L-файла.</summary>
    public required string LInvoiceFilename { get; set; }
}
