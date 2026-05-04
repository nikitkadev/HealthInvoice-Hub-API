using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class NazEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUId { get; set; }

    [XmlElement("NAZ_N")]
    public int NazN { get; set; }

    [XmlElement("NAZ_R")]
    public byte NazR { get; set; }

    [XmlElement("NAZ_IDDOKT")]
    public string NazIddokt { get; set; } = string.Empty;

    [XmlElement("NAZ_V")]
    public byte? NazV { get; set; }

    [XmlElement("NAZ_USL")]
    public string? NazUsl { get; set; }

    [XmlElement("NAPR_DATE")]
    public DateTime? NaprDate { get; set; }

    [XmlElement("NAPR_MO")]
    public string? NaprMo { get; set; }

    [XmlElement("NAZ_PMP")]
    public int? NazPmp { get; set; }

    [XmlElement("NAZ_PK")]
    public string? NazPk { get; set; }

}