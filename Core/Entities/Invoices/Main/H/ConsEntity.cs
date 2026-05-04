using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class ConsEntity
{
    [XmlIgnore]
    public int Uid { get; set; }  

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("PR_CONS")]
    public byte PrCons { get; set; }

    [XmlElement("DT_CONS")]
    public DateTime? DtCons { get; set; }
}
