using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class OnkUslEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int OnkSluchUid { get; set; }

    [XmlElement("USL_TIP")]
    public byte UslTip { get; set; }

    [XmlElement("HIR_TIP")]
    public byte? HirTip { get; set; }

    [XmlElement("LEK_TIP_L")]
    public byte? LekTipL { get; set; }

    [XmlElement("LEK_TIP_V")]
    public byte? LekTipV { get; set; }

    [XmlElement("LEK_PR")]
    public List<LekPrEntity>? LekPrs { get; set; }

    [XmlElement("PPTR")]
    public byte? PPTR { get; set; }

    [XmlElement("LUCH_TIP")]
    public byte? LuchTip { get; set; }
}