using System.Xml.Serialization;

namespace HealthInvoice.Core.Dtos.Reports;

/// <summary>
/// XML-отчёт форматного контроля (ФК).
/// </summary>
[XmlRoot("FLK_P")]
public class ReportFormatFDto
{
    /// <summary>Имя файла (FNAME).</summary>
    [XmlElement("FNAME")]
    public string FName { get; set; } = string.Empty;

    /// <summary>Дополнительное имя файла (FNAME_1).</summary>
    [XmlElement("FNAME_1")]
    public string FName1 { get; set; } = string.Empty;

    /// <summary>Список ошибок.</summary>
    [XmlElement("PR")]
    public List<PR_F> PRs { get; set; } = [];
}

/// <summary>
/// Элемент отчёта с ошибкой.
/// </summary>
public class PR_F
{
    /// <summary>Код ошибки (0 — нет ошибки).</summary>
    [XmlElement("OSHIB")]
    public int Oshib { get; set; } = 0;

    /// <summary>Текст ошибки/комментарий.</summary>
    [XmlElement("COMMENT")]
    public string Comment { get; set; } = "Без ошибок.";
}
