using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class PacientEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlElement("ID_PAC")]
    public string IdPac { get; set; } = string.Empty;

    [XmlElement("VPOLIS")]
    public byte Vpolis { get; set; }

    [XmlElement("SPOLIS")]
    public string? Spolis { get; set; }

    [XmlElement("NPOLIS")]
    public string Npolis { get; set; } = string.Empty;

    [XmlElement("ENP")]
    public string? Enp { get; set; }

    [XmlElement("ST_OKATO")]
    public string? StOkato { get; set; }

    [XmlElement("SMO")]
    public string? Smo { get; set; }

    [XmlElement("SMO_OGRN")]
    public string? SmoOgrn { get; set; }

    [XmlElement("SMO_OK")]
    public string? SmoOk { get; set; }

    [XmlElement("SMO_NAM")]
    public string? SmoNam { get; set; }

    [XmlElement("INV")]
    public byte? Inv { get; set; }

    [XmlElement("MSE")]
    public byte? Mse { get; set; }

    [XmlElement("NOVOR")]
    public string Novor { get; set; } = string.Empty;

    [XmlElement("VNOV_D")]
    public int? VnovD{ get; set; }

    [XmlElement("SOC")]
    public string Soc { get; set;  } = string.Empty;

    [XmlElement("NEXT_D")]
    public int? NextD { get; set; }

    [XmlElement("MO_PR")]
    public string? MoPr { get; set; }

    [XmlElement("VZ")]
    public string? VZ { get; set; }
}
