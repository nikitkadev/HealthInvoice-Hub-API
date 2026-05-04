using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using HealthInvoice.Core.Entities.Invoices.Additional;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class KsgKpgEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("N_KSG")]
    public string NKsg { get; set; } = string.Empty;
    
    [XmlElement("VER_KSG")]
    public int VerKsg { get; set; }
    
    [XmlElement("KSG_PG")]
    public byte KsgPg { get; set; }

    [XmlElement("N_KPG")]
    public string? NKpg { get; set; }

    [XmlElement("KOEF_Z")]
    public decimal KoefZ { get; set; }
    
    [XmlElement("KOEF_UP")]
    public decimal KoefUp { get; set; }
    
    [XmlElement("BZTSZ")]
    public decimal Bztsz { get; set; }
    
    [XmlElement("KOEF_D")]
    public decimal KoefD { get; set; }
    
    [XmlElement("KOEF_U")]
    public decimal KoefU { get; set; }
    
    [XmlElement("K_ZP")]
    public decimal? KZp { get; set; }

    [XmlElement("CRIT")]
    [NotMapped]
    public List<string>? Crits { get; set; }

    [XmlElement("SL_K")]
    public byte SlK { get; set;  }

    [XmlElement("IT_SL")]
    public decimal? ItSl { get; set; }

    [XmlElement("SL_KOEF")]
    public List<SlKoefEntity>? SlKoefs { get; set; }

    [XmlElement("PR_PR")]
    public string PrPr { get; set; } = string.Empty;

    [XmlElement("KOEF_PR")]
    public decimal? KoefPr { get; set; }


    [XmlIgnore]
    public List<CritEntity> CritEntities { get; set; } = [];
}
