namespace HealthInvoice.Core.Dtos.Rcontrol.Tables;

/// <summary>
/// Краткая сводка по счёту (для административной панели).
/// </summary>
public class InvoiceOverallDto
{
    /// <summary>Статус счёта (1 — успешно, -1 — ошибка, 2 — ожидает и т.д.).</summary>
    public short Status { get; set; }

    /// <summary>Номер счёта.</summary>
    public string NSchet { get; set; } = string.Empty;

    /// <summary>Дата выставления счёта.</summary>
    public DateTime DSchet { get; set; }

    /// <summary>Предъявленная сумма.</summary>
    public decimal Summav { get; set; }

    /// <summary>Количество случаев (записей).</summary>
    public int? SdZ { get; set; }

    /// <summary>Уникальный идентификатор счёта.</summary>
    public int SchetUid { get; set; }
}
