using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class OnkSlEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("DS1_T")]
    public byte? Ds1T { get; set; }

    [XmlElement("STAD")]
    public int? Stad { get; set; }

    [XmlElement("ONK_T")]
    public int? OnkT { get; set; }

    [XmlElement("ONK_N")]
    public int? OnkN { get; set; }

    [XmlElement("ONK_M")]
    public int? OnkM { get; set; }

    [XmlElement("MTSTZ")]
    public byte? Mtstz { get; set; }

    [XmlElement("B_DIAG")]
    public List<BDiagEntity>? BDiags { get; set; }

    [XmlElement("B_PROT")]
    public List<BProtEntity>? BProts { get; set; }

    [XmlElement("SOD")]
    public float? Sod { get; set; }

    [XmlElement("K_FR")]
    public byte? KFr { get; set; }

    [XmlElement("WEI")]
    public float? Wei { get; set; }

    [XmlElement("HEI")]
    public int? Hei { get; set; }

    [XmlElement("BSA")]
    public float? Bsa { get; set; }

    [XmlElement("ONK_USL")]
    public List<OnkUslEntity>? OnkUsls { get; set; }

}
