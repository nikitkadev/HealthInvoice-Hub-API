using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

[XmlRoot("ZL_LIST")]
public class ZlListEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public DateTime UploadDate { get; set; } = DateTime.Now;

    [XmlIgnore]
    public string? Uploader { get; set; }

    [XmlIgnore]
    public short Status { get; set; } = -1;

    [XmlElement("ZGLV")]
    public ZglvEntity? Zglv { get; set; }

    [XmlElement("SCHET")]
    public SchetEntity? Schet { get; set; }

    [XmlElement("ZAP")]
    public List<ZapEntity> Zaps { get; set; } = [];
}
