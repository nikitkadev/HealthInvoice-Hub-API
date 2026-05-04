using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class SchetEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int ZlListUid { get; set; }

    [XmlElement("CODE")]
    public long Code { get; set; }

    [XmlElement("CODE_MO")]
    public string CodeMO { get; set; } = string.Empty;

    [XmlElement("YEAR")]
    public int Year { get; set; }

    [XmlElement("MONTH")]
    public byte Month { get; set; }
    
    [XmlElement("NSCHET")]
    public string Nschet { get; set; } = string.Empty;
    
    [XmlElement("DSCHET")]
    public DateTime Dschet { get; set; }
    
    [XmlElement("PLAT")]
    public string? Plat { get; set; }
    
    [XmlElement("SUMMAV")]
    public decimal Summav { get; set; }

    [XmlElement("COMENTS")]
    public string? Coments { get; set; }

    [XmlElement("SUMMAP")]
    public decimal? Summap { get; set; } = 0m;

    [XmlElement("SANK_MEK")]
    public decimal? SankMek { get; set; } = 0m;

    [XmlElement("SANK_MEE")]
    public decimal? SankMee { get; set; } = 0m;

    [XmlElement("SANK_EKMP")]
    public decimal? SankEkmp { get; set; } = 0m;

    [XmlElement("SMO_SUMMAP")]
    public decimal? SmoSummap { get; set; } = 0m;

    [XmlElement("SMO_SANK_MEK")]
    public decimal? SmoSankMek { get; set; } = 0m;

    [XmlElement("SMO_SANK_MEE")]
    public decimal? SmoSankMee { get; set; } = 0m;

    [XmlElement("SMO_SANK_EKMP")]
    public decimal? SmoSankEkmp { get; set; } = 0m;

    [XmlElement("DISP")]
    public string? Disp { get; set; }
}
