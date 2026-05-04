namespace HealthInvoice.Core.Dtos.Rcontrol.Tables;

/// <summary>Информация о случае лечения (случае оказания медицинской помощи).</summary>
public class CaseDto
{
    /// <summary>Профиль.</summary>
    public int? Profil { get; set; }

    /// <summary>Детский (дет.).</summary>
    public short Det { get; set; }

    /// <summary>Код специальности (специальность врача).</summary>
    public int Prvs { get; set; }

    /// <summary>Дата начала лечения.</summary>
    public DateTime StartingAt { get; set; }

    /// <summary>Дата окончания лечения.</summary>
    public DateTime EndingAt { get; set; }

    /// <summary>Диагноз (основной).</summary>
    public string Ds1 { get; set; } = string.Empty;

    /// <summary>Количество услуг.</summary>
    public decimal? EdCol { get; set; }

    /// <summary>Тариф.</summary>
    public decimal? Tarif { get; set; }

    /// <summary>Предъявлено к оплате.</summary>
    public decimal SumM { get; set; }

    /// <summary>Принято ТФОМС.</summary>
    public decimal? Sump { get; set; }

    /// <summary>Принято СМО.</summary>
    public decimal? SmoSump { get; set; }
}