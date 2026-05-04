using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class InjEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int LekPrUid { get; set; }

    [XmlElement("DATE_INJ")]
    public DateTime DateInj { get; set; }

    [XmlElement("KV_INJ")]
    public decimal? KvInj { get; set; }

    [XmlElement("KIZ_INJ")]
    public decimal? KizInj { get; set; }

    [XmlElement("S_INJ")]
    public decimal? SInj { get; set; }

    [XmlElement("SV_INJ")]
    public decimal? SvInj { get; set; }

    [XmlElement("SIZ_INJ")]
    public decimal? SizInj { get; set; }

    [XmlElement("RED_INJ")]
    public bool? RedInj { get; set; }
}
