using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.L;

public class PersEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int PersListUid { get; set; }

    [XmlElement("ID_PAC")]
    public string IdPac { get; set; } = string.Empty;

    [XmlElement("FAM")]
    public string Fam { get; set; } = string.Empty;

    [XmlElement("IM")]
    public string Im { get; set; } = string.Empty;

    [XmlElement("OT")]
    public string Ot { get; set; } = string.Empty;

    [XmlElement("W")]
    public byte W { get; set; }

    [XmlElement("DR")]
    public DateTime Dr { get; set; }

    [XmlElement("DOST")]
    public byte? Dost { get; set; }

    [XmlElement("TEL")]
    public string? Tel { get; set; }

    [XmlElement("FAM_P")]
    public string? FamP { get; set; }

    [XmlElement("IM_P")]
    public string? ImP { get; set; }

    [XmlElement("OT_P")]
    public string? OtP { get; set; }

    [XmlElement("W_P")]
    public byte? WP { get; set; }

    [XmlElement("DR_P")]
    public DateTime? DrP { get; set; }

    [XmlElement("DOST_P")]
    public byte? DostP { get; set; }

    [XmlElement("MR")]
    public string? MR { get; set; }

    [XmlElement("DOCTYPE")]
    public string? Doctype { get; set; }

    [XmlElement("DOCSER")]
    public string? Docser { get; set; }

    [XmlElement("DOCNUM")]
    public string? Docnum { get; set; }

    [XmlElement("DOCDATE")]
    public DateTime? DocDate { get; set; }

    [XmlElement("DOCORG")]
    public string? DocOrg { get; set; }

    [XmlElement("SNILS")]
    public string? Snils { get; set; }

    [XmlElement("OKATOG")]
    public string? Okatog { get; set; }

    [XmlElement("OKATOP")]
    public string? Okatop { get; set; }

    [XmlElement("COMENTP")]
    public string? Comentp { get; set; }

}
