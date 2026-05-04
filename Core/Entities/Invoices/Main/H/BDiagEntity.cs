using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class BDiagEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int OnkSluchUid { get; set; }

    [XmlElement("DIAG_DATE")]
    public DateTime? DiagDate { get; set; }

    [XmlElement("DIAG_TIP")]
    public byte? DiagTip { get; set; }

    [XmlElement("DIAG_CODE")]
    public int? DiagCode { get; set; }

    [XmlElement("DIAG_RSLT")]
    public int? DiagRslt { get; set; }

    [XmlElement("REC_RSLT")]
    public byte? RecRslt { get; set; }
}
