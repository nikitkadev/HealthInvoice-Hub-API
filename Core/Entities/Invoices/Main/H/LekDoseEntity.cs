using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class LekDoseEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchLekPrUid { get; set; }

    [XmlElement("ED_IZM")]
    public string EdIzm { get; set; } = string.Empty;

    [XmlElement("DOSE_INJ")]
    public decimal DoseInj { get; set; }

    [XmlElement("METHOD_INJ")]
    public string MethodInj { get; set; } = string.Empty;

    [XmlElement("COL_INJ")]
    public int ColInj { get; set; }
}
