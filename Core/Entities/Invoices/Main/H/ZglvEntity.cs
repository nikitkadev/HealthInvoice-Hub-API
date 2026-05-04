using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class ZglvEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int ZlListUid { get; set; }

    [XmlElement("VERSION")]
    public string? Version { get; set; }

    [XmlElement("DATA")]
    public DateTime? Data { get; set; }

    [XmlElement("FILENAME")]
    public string? FileName { get; set; }

    [XmlElement("SD_Z")]
    public int? SdZ { get; set; }
}
