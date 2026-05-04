using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class SankEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("S_CODE")]
    public string SCode { get; set; } = string.Empty;

    [XmlElement("S_SUM")]
    public decimal SSum { get; set; }

    [XmlElement("S_TIP")]
    public string STip { get; set; } = string.Empty;

    //Any
    //[XmlElement("SL_ID")]
    //public string? SlID { get; set; }

    [XmlElement("S_OSN")]
    public int SOsn { get; set; }

    [XmlElement("S_ED_COL")]
    public decimal SEDCol { get; set; }

    [XmlElement("DATE_ACT")]
    public DateTime DateAct { get; set; }

    [XmlElement("NUM_ACT")]
    public string NumAct { get; set; } = string.Empty;

    [XmlElement("CODE_EXP")]
    public string? CodeExp { get; set; }

    [XmlElement("S_COM")]
    public string? SCom { get; set; }

    [XmlElement("S_IST")]
    public short SIst { get; set; }
}
