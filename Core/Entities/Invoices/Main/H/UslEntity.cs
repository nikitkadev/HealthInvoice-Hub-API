using System.Xml.Serialization;

namespace HealthInvoice.Core.Entities.Invoices.Main.H;

public class UslEntity
{
    [XmlIgnore]
    public int Uid { get; set; }

    [XmlIgnore]
    public int SluchUid { get; set; }

    [XmlElement("IDSERV")]
    public string Idserv { get; set; } = string.Empty;

    [XmlElement("LPU")]
    public string Lpu { get; set; } = string.Empty;

    [XmlElement("LPU_1")]
    public string? Lpu1 { get; set; }

    [XmlElement("PODR")]
    public long? Podr { get; set; }

    [XmlElement("PROFIL")]
    public int Profil { get; set; }

    [XmlElement("VID_VME")]
    public string? VidVme { get; set; }

    [XmlElement("DET")]
    public byte Det { get; set; }
    
    [XmlElement("DATE_IN")]
    public DateTime DateIn { get; set; }

    [XmlElement("DATE_OUT")]
    public DateTime DateOut { get; set; }

    [XmlElement("P_OTK")]
    public byte? POtk { get; set; }

    [XmlElement("DS")]
    public string DS { get; set; } = string.Empty;

    [XmlElement("CODE_USL")]
    public string CodeUsl { get; set; } = string.Empty;

    [XmlElement("KOL_USL")]
    public decimal KolUsl { get; set; }

    [XmlElement("TARIF")]
    public decimal? Tarif { get; set; }

    [XmlElement("SUMV_USL")]
    public decimal SumvUsl { get; set; }

    [XmlElement("MED_DEV")]
    public List<MedDevEntity>? MedDevs { get; set; }

    [XmlElement("MR_USL_N")]
    public List<MrUslNEntity>? MrUslNs { get; set; }

    [XmlElement("USLDOPPARAM")]
    public UslDopParamEntity? UslDopParams { get; set; }

    [XmlElement("PRVS")]
    public int PRVS { get; set; }

    [XmlElement("CODE_MD")]
    public string CodeMd { get; set; } = string.Empty;

    [XmlElement("NPL")]
    public short? NPL { get; set; }
    
    [XmlElement("COMENTU")]
    public string? Comentu { get; set; }

    [XmlIgnore]
    public string? VolumeCode { get; set; }
}
