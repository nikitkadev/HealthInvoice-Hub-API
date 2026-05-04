using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.L;

[XmlRoot("PERS_LIST")]
public class PersListEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public DateTime UploadDate { get; set; } = DateTime.Now;

    [XmlIgnore]
    public string? Uploader { get; set; }

    [XmlIgnore]
    public short? Status { get; set; } = -1;

    [XmlElement("ZGLV")]
    public PZglvEntity? PZglv { get; set; }

    [XmlElement("PERS")]
    public List<PersEntity>? Pers { get; set; } = [];
}
