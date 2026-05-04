using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class ZSlEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int ZapUid { get; set; }

    [XmlElement("IDCASE")]
    public long Idcase { get; set; }
    
    [XmlElement("USL_OK")]
    public int UslOk { get; set; }
    
    [XmlElement("VIDPOM")]
    public int VidPom { get; set; }

    [XmlElement("FOR_POM")]
    public byte ForPom { get; set; }

    [XmlElement("NPR_MO")]
    public string? NprMo { get; set; }

    [XmlElement("NPR_DATE")]
    public DateTime? NprDate { get; set; }

    [XmlElement("NPR_NUM")]
    public string? NprNum { get; set; }

    [XmlElement("LPU")]
    public string Lpu { get; set; } = string.Empty;

    [XmlElement("VBR")]
    public byte? Vbr { get; set; }

    [XmlElement("EVENING_TIME")]
    public byte? EveningTime { get; set; }

    [XmlElement("DATE_Z_1")]
    public DateTime DateZ1 { get; set; }

    [XmlElement("DATE_Z_2")]
    public DateTime DateZ2 { get; set; }

    [XmlElement("P_OTK")]
    public byte? POtk { get; set; }

    [XmlElement("RSLT_D")]
    public byte? RsltD { get; set; }

    [XmlElement("KD_Z")]
    public int? KdZ { get; set; }

    [XmlElement("VNOV_M")]
    public int? VnovM { get; set; }

    [XmlElement("RSLT")]
    public int Rslt { get; set; }
    
    [XmlElement("ISHOD")]
    public int Ishod { get; set; }

    [XmlElement("OS_SLUCH")]
    public byte? OsSluch { get; set; }

    [XmlElement("VB_P")]
    public byte? VbP { get; set; }
    
    [XmlElement("SL")]
    public List<SlEntity>? Sls { get; set; }
    
    [XmlElement("IDSP")]
    public short Idsp { get; set; }
    
    [XmlElement("SUMV")]
    public decimal SumV { get; set; }

    [XmlElement("OPLATA")]
    public byte? Oplata { get; set; } 

    [XmlElement("SUMP")]
    public decimal? Sump { get; set; } 

    [XmlElement("SMO_SUMP")]
    public decimal? SmoSump { get; set; } 

    [XmlElement("SANK_IT")]
    public decimal? SankIt { get; set; }

    [XmlElement("SMO_SANK_IT")]
    public decimal? SmoSankIt { get; set; }

}
