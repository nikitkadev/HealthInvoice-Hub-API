using HealthInvoice.Core.Common;

namespace HealthInvoice.Core.Dtos.Background;

/// <summary>
/// Сообщение для фоновой операции с базой данных (проверка МЭК).
/// </summary>
/// <param name="SchetUid">Uid проверяемого счета.</param>
/// <param name="JournalType">Тип выбранного журнала пользователем.</param>
public record InvoiceLogicControlMessage(
    int SchetUid,
    JournalType JournalType);
