using HealthInvoice.Core.Common;

namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Параметры для полной проверки счета (мета + формат).
/// </summary>
public class InvoiceValidationParameters
{
    /// <summary>Тип журнала (СМОРХ / ИНОГОРОД).</summary>
    public required JournalType JournalType { get; set; }

    /// <summary>Список архивов для проверки.</summary>
    public required List<InvoiceArchive> Archives { get; set; }
}

/// <summary>
/// Архив счёта для проверки.
/// </summary>
public class InvoiceArchive
{
    /// <summary>Поток архива.</summary>
    public required Stream ArchiveStream { get; set; }

    /// <summary>Имя архива.</summary>
    public required string ArchiveFilename { get; set; }

    /// <summary>Код организации.</summary>
    public required string OrganizationCode { get; set; }
}
