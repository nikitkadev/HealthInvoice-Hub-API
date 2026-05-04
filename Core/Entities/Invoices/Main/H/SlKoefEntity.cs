using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class SlKoefEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int KsgKpgUid { get; set; }

    [XmlElement("IDSL")]
    public string Idsl { get; set; } = string.Empty;

    [XmlElement("Z_SL")]
    public decimal ZSl { get; set; }

}
