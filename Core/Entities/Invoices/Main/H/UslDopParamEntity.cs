using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H
{
    public class UslDopParamEntity
    {
        [XmlIgnore]
        public int Uid { get; set; }

        [XmlIgnore]
        public int UslUid { get; set; }

        [XmlElement("KOD")]
        public string Kod { get; set; } = string.Empty;
    }
}
