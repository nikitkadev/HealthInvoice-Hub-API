using HealthInvoice.Core.Common;

namespace HealthInvoice.Core.Dtos.Background;

/// <summary>
/// Сообщение для фоновой операции с базой данных (вставка/обновление счёта).
/// </summary>
public record InvoiceDbOperationMessage(
    string FileName,
    string FilePath,
    string Uploader,
    int? SchetUid,
    JournalType JournalType,
    DbOperation DbOperation
)
{
    /// <summary>Время отправки сообщения.</summary>
    public string SendingAt { get; init; } = DateTime.Now.ToString("G");
}