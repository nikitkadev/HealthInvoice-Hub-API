namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Запрос на отправку счетов в очередь (запись/перезапись в БД).
/// </summary>
public record InvoicesUpsertRequest(
    List<InvoiceQueueItem> Items,
    int JournalType
);

/// <summary>
/// Элемент очереди для записи/перезаписи счёта.
/// </summary>
public record InvoiceQueueItem(
    string Filename,
    string FilePath,
    int? SchetUid = null
);
