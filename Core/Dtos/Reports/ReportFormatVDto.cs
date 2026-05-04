using System.Xml.Serialization;

namespace HealthInvoice.Core.Dtos.Reports;

/// <summary>
/// XML-отчёт логического контроля (МЭК / ЛК).
/// </summary>
[XmlRoot("FLK_P")]
public class ReportFormatVDto
{
    /// <summary>Имя файла.</summary>
    [XmlElement("FNAME")]
    public string FName { get; set; } = string.Empty;

    /// <summary>Дополнительное имя файла.</summary>
    [XmlElement("FNAME_1")]
    public string FName1 { get; set; } = string.Empty;

    /// <summary>Список ошибок логического контроля.</summary>
    [XmlElement("PR")]
    public List<PR_V> PR { get; set; } = [];
}

/// <summary>
/// Элемент отчёта логического контроля (МЭК).
/// </summary>
public class PR_V
{
    /// <summary>Код ошибки.</summary>
    [XmlElement("OSHIB")]
    public short Oshib { get; set; } = -1;

    /// <summary>Имя полиса.</summary>
    [XmlElement("IM_POL")]
    public string ImPl { get; set; } = string.Empty;

    /// <summary>Базовый элемент.</summary>
    [XmlElement("BAS_EL")]
    public string BasEl { get; set; } = string.Empty;

    /// <summary>Номер записи в реестре.</summary>
    [XmlElement("N_ZAP")]
    public long NZap { get; set; }

    /// <summary>Номер случая.</summary>
    [XmlElement("IDCASE")]
    public long IdCase { get; set; }

    /// <summary>Идентификатор услуги.</summary>
    [XmlElement("SL_ID")]
    public string SlId { get; set; } = string.Empty;

    /// <summary>Фамилия пациента.</summary>
    [XmlElement("FAM")]
    public string Fam { get; set; } = string.Empty;

    /// <summary>Имя пациента.</summary>
    [XmlElement("IM")]
    public string Im { get; set; } = string.Empty;

    /// <summary>Отчество пациента.</summary>
    [XmlElement("OT")]
    public string Ot { get; set; } = string.Empty;

    /// <summary>Текст ошибки/комментарий.</summary>
    [XmlElement("COMMENT")]
    public string Comment { get; set; } = "Без ошибок.";
}