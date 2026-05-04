using HealthInvoice.Core.Entities.Invoices.Additional;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class SlEntity
{
    [XmlIgnore]
    public int Uid { get; set; }
    
    [XmlIgnore]
    public int ZSlUid { get; set; }
    
    [XmlElement("SL_ID")]
    public string SlId { get; set; } = string.Empty;

    [XmlElement("VID_HMP")]
    public string? VidHmp { get; set; }

    [XmlElement("METOD_HMP")]
    public int? MetodHmp { get; set; }
    
    [XmlElement("LPU_1")]
    public string? Lpu1 { get; set; }
    
    [XmlElement("PODR")]
    public long? Podr { get; set; }
    
    [XmlElement("PROFIL")]
    public int Profil { get; set; }
    
    [XmlElement("PROFIL_K")]
    public int? ProfilK { get; set; }
    
    [XmlElement("DET")]
    public short Det { get; set; }

    [XmlElement("TAL_D")]
    public DateTime? TalD { get; set; }

    [XmlElement("TAL_NUM")]
    public string? TalNum { get; set; }

    [XmlElement("TAL_P")]
    public DateTime? TalP { get; set; }

    [XmlElement("P_CEL")]
    public string? PCel { get; set; }

    [XmlElement("MOP")]
    public string? Mop { get; set; }

    [XmlElement("NHISTORY")]
    public string NHistory { get; set; } = string.Empty;
    
    [XmlElement("P_PER")]
    public short? PPer { get; set; }
    
    [XmlElement("DATE_1")]
    public DateTime Date1 { get; set; }
    
    [XmlElement("DATE_2")]
    public DateTime Date2 { get; set; }
    
    [XmlElement("KD")]
    public int? KD { get; set; }

    [XmlElement("WEI")]
    public decimal? Wei { get; set; }

    [XmlElement("DS0")]
    public string? Ds0 { get; set; }
    
    [XmlElement("DS1")]
    public string Ds1 { get; set; } = string.Empty;

    [XmlElement("DS1_PR")]
    public byte? Ds1_Pr { get; set; }

    [XmlElement("DS2")]
    [NotMapped]
    public List<string>? Ds2 { get; set; }

    [XmlElement("DS3")]
    [NotMapped]
    public List<string>? Ds3 { get; set; }

    [XmlElement("C_ZAB")]
    public byte? CZab { get; set; }

    [XmlElement("DS_ONK")]
    public byte? DsOnk { get; set; }

    [XmlElement("PR_D_N")]
    public byte? PrDN { get; set; }

    [XmlElement("PROF_M")]
    public byte? ProfM { get; set; }

    [XmlElement("DS2_N")]
    public List<Ds2NEntity>? Ds2Ns { get; set; }

    [XmlElement("NAZ")]
    public List<NazEntity>? Nazs { get; set; }
    
    [XmlElement("DN")]
    public byte? Dn { get; set; }

    [XmlElement("CODE_MES1")]
    public string? CodeMes1 { get; set; }

    [XmlElement("CODE_MES2")]
    public string? CodeMes2 { get; set; }

    [XmlElement("NAPR")]
    public List<NaprEntity>? Naprs { get; set; }

    [XmlElement("CONS")]
    public List<ConsEntity>? Cons { get; set; }

    [XmlElement("ONK_SL")]
    public OnkSlEntity? OnkSl { get; set; }

    [XmlElement("KSG_KPG")]
    public KsgKpgEntity? KsgKpg { get; set; }

    [XmlElement("REAB")]    
    public byte? Reab { get; set; }

    [XmlElement("PRVS")]
    public int Prvs { get; set; }
    
    [XmlElement("VERS_SPEC")]
    public string VersSpec { get; set; } = string.Empty;
    
    [XmlElement("IDDOKT")]
    public string Iddokt { get; set; } = string.Empty;
    
    [XmlElement("ED_COL")]
    public decimal? EdCol { get; set; }
    
    [XmlElement("TARIF")]
    public decimal? Tarif { get; set; }
    
    [XmlElement("SUM_M")]
    public decimal SumM { get; set; }

    [XmlElement("SLUCH_LEK_PR")]
    public List<SlLekPrEntity>? SluchLekPrs { get; set; }

    [XmlElement("SMO_SUMP")]
    public decimal? SmoSump { get; set; }

    [XmlElement("SANK")]
    public List<SankEntity>? Sanks { get; set; }

    [XmlElement("USL")]
    public List<UslEntity>? Usls { get; set; }

    [XmlElement("COMENTSL")]
    public string? Comentsl { get; set; }

    [XmlIgnore]
    public decimal? SankMek { get; set; }

    [XmlIgnore]
    public decimal? SankMee { get; set; }

    [XmlIgnore]
    public decimal? SankEkmp { get; set; }

    [XmlElement("LPU_LEVEL")]
    public string? LpuLevel { get; set; }


    [XmlIgnore]
    public List<Ds2Entity> Ds2s { get; set; } = [];

    [XmlIgnore]
    public List<Ds3Entity> Ds3s { get; set; } = [];
}
