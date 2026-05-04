using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class BProtEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int OnkSluchUid { get; set; }

    [XmlElement("PROT")]
    public byte Prot { get; set; }

    [XmlElement("D_PROT")]
    public DateTime DProt { get; set; }
}
