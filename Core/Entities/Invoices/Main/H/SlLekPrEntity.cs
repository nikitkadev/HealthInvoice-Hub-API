using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class SlLekPrEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("DATA_INJ")]
    public DateTime DataInj { get; set; }

    [XmlElement("CODE_SH")]
    public string CodeSh { get; set; } = string.Empty;

    [XmlElement("REGNUM")]
    public string? Regnum { get; set; }

    [XmlElement("COD_MARK")]
    public string? CodMark { get; set; }

    [XmlElement("LEK_DOSE")]
    public LekDoseEntity? LekDose { get; set; }
}
