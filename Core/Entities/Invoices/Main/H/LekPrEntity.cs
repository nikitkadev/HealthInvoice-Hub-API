using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using HealthInvoice.Core.Entities.Invoices.Additional;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class LekPrEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int OnkUslUid { get; set; }

    [XmlElement("REGNUM")]
    public string Regnum { get; set; } = string.Empty;

    [XmlElement("REGNUM_DOP")]
    public string? RegnumDop { get; set; }

    [XmlElement("CODE_SH")]
    public string? CodeSh { get; set; }

    [XmlElement("DATE_INJ")]
    [NotMapped]
    public List<DateTime> DateInjs { get; set; } = [];

    [XmlElement("INJ")]
    public List<InjEntity>? Injs { get; set; }


    [XmlIgnore]
    public List<DateInjEntity> DateInjEntities { get; set; } = [];

}
