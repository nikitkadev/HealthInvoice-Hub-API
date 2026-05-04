using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class MedDevEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int UslUid { get; set; }

    [XmlElement("DATE_MED")]
    public DateTime DateMed { get; set; }

    [XmlElement("CODE_MEDDEV")]
    public int CodeMeddev { get; set; }

    [XmlElement("NUMBER_SER")]
    public string NumberSer { get; set; } = string.Empty;
}
