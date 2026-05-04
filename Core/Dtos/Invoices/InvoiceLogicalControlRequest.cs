namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Запрос на запуск логического контроля (МЭК) для списка счетов.
/// </summary>
public class SelectedInvoiceRequest
{
    /// <summary>Список UID счетов для проверки.</summary>
    public List<int> SchetUids { get; set; } = [];

    /// <summary>Тип журнала (СМОРХ / ИНОГОРОД).</summary>
    public int JournalType { get; set; } = 0;
}