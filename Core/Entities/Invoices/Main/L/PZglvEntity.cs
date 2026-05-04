using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.L;

public class PZglvEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int PersListUid { get; set; }

    [XmlElement("VERSION")]
    public string? Version { get; set; }

    [XmlElement("DATA")]
    public DateTime? Data { get; set; }

    [XmlElement("FILENAME")]
    public string? FileName { get; set; }

    [XmlElement("FILENAME1")]
    public string? FileName1 { get; set; }
}
