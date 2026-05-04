using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class Ds2NEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("DS2")]
    public string Ds2 { get; set; } = string.Empty;

    [XmlElement("DS2_PR")]
    public byte? Ds2PR { get; set; }

    [XmlElement("PR_DS2_N")]
    public byte PRDs2N { get; set; }
}