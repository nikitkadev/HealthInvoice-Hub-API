namespace HealthInvoice.Core.Dtos.Rcontrol.Filters;

/// <summary>Транспортный класс для передачи на фронтенд информации о расчетных периодах.</summary>
public class BillingPeriodDto
{
    /// <summary>Год расчетного периода.</summary>
    public int Year { get; set; }

    /// <summary>Месяц расчетного периода.</summary>
    public byte Month { get; set; }
}
