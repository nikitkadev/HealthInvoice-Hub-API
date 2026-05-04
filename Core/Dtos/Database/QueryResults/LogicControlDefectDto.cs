namespace HealthInvoice.Core.Dtos.Database.QueryResults;

/// <summary>Дефекты логического контроля (ЛК / МЭК) для счёта.</summary>
public class LogicControlDefectDto
{
    public string Comment { get; set; } = string.Empty;

    /// <summary>Базовый элемент.</summary>
    public string BasEl { get; set; } = string.Empty;

    /// <summary>Имя полиса.</summary>
    public string ImPol { get; set; } = string.Empty;

    /// <summary>Код ошибки.</summary>
    public short Kod { get; set; }

    /// <summary>Номер записи.</summary>
    public long NZap { get; set; }

    /// <summary>Номер случая.</summary>
    public long IdCase { get; set; }

    /// <summary>Идентификатор услуги.</summary>
    public string SlId { get; set; } = string.Empty;

    /// <summary>Фамилия пациента.</summary>
    public string Fam { get; set; } = string.Empty;

    /// <summary>Имя пациента.</summary>
    public string Im { get; set; } = string.Empty;

    /// <summary>Отчество пациента.</summary>
    public string Ot { get; set; } = string.Empty;

    /// <summary>Дата рождения пациента.</summary>
    public DateTime? Dr { get; set; }

    /// <summary>Начало периода.</summary>
    public DateTime? Date1 { get; set; }

    /// <summary>Окончание периода.</summary>
    public DateTime? Date2 { get; set; }

    /// <summary>Идентификатор доктора.</summary>
    public string Iddokt { get; set; } = string.Empty;
}