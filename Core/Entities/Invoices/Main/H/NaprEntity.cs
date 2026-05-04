using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class NaprEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUId { get; set; }

    [XmlElement("NAPR_DATE")]
    public DateTime NaprDate { get; set; }

    [XmlElement("NAPR_MO")]
    public string? NaprMo { get; set; }

    [XmlElement("NAPR_V")]
    public byte NaprV { get; set; }

    [XmlElement("MET_ISSL")]
    public byte? MetIssl { get; set; }

    [XmlElement("NAPR_USL")]
    public string? NaprUsl { get; set; }
}