namespace HealthInvoice.Core.Dtos.Rcontrol.Tables;

/// <summary>
/// Информация о завершённом случае лечения (для административной панели).
/// </summary>
public class FinishedCaseDto
{

    /// <summary>Уникальный идентификатор пациента (PC_UID).</summary>
    public int PacientUid { get; set; }

    /// <summary>Уникальный идентификатор персоны (PERS_UID).</summary>
    public int PersUid { get; set; }

    /// <summary>Уникальный идентификатор случая (Z_SL_UID).</summary>
    public int ZSlUid { get; set; }


    /// <summary>Номер позиции в реестре (IDCASE).</summary>
    public long PositionNumber { get; set; }

    /// <summary>Номер записи (N_ZAP).</summary>
    public long RecordNumber { get; set; }


    /// <summary>Фамилия пациента.</summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>Имя пациента.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Отчество пациента.</summary>
    public string Patronymic { get; set; } = string.Empty;

    /// <summary>Условие оказания медицинской помощи (USL_OK).</summary>
    public int UslOk { get; set; }


    /// <summary>Серия полиса ОМС (может отсутствовать).</summary>
    public string? SPolis { get; set; }

    /// <summary>Номер полиса ОМС.</summary>
    public string NPolis { get; set; } = string.Empty;


    /// <summary>Предъявлено к оплате (SUMV).</summary>
    public decimal Sumv { get; set; }

    /// <summary>Принято ТФОМС (SUMP).</summary>
    public decimal? Sump { get; set; }

    /// <summary>Принято СМО (SMO_SUMP).</summary>
    public decimal? SmoSump { get; set; }
}
