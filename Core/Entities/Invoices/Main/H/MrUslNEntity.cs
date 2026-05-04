using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class MrUslNEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int UslUid { get; set; }

    [XmlElement("MR_N")]
    public int MrN { get; set; }

    [XmlElement("PRVS")]
    public int PRVS { get; set; }

    [XmlElement("CODE_MD")]
    public string CodeMd { get; set; } = string.Empty;
}
