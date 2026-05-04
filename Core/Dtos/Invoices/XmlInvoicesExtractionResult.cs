namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Результат извлечения XML-счетов из архива.
/// </summary>
public class XmlInvoicesExtractionResult
{
    /// <summary>Содержимое H-счета.</summary>
    public required MemoryStream HInvoiceStream { get; set; }

    /// <summary>Содержимое L-счета.</summary>
    public required MemoryStream LInvoiceStream { get; set; }

    /// <summary>Имя файла H-счета.</summary>
    public required string HInvoiceFilename { get; set; }

    /// <summary>Имя файла L-счета.</summary>
    public required string LInvoiceFilename { get; set; }
}