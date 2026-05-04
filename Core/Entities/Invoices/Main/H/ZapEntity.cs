using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class ZapEntity
{
    [XmlIgnore]
    public int Uid { get; set; }
    
    [XmlIgnore]
    public int ZlListUid { get; set; }

    [XmlIgnore]
    public int PacientUid { get; set; }

    [XmlElement("N_ZAP")]    
    public long NZap {  get; set; }

    [XmlElement("PR_NOV")]
    public short PrNov { get; set; }
   
    [XmlElement("PACIENT")]
    public PacientEntity? Pacient { get; set; }

    [XmlElement("Z_SL")]
    public ZSlEntity? ZSl { get; set; }
}
