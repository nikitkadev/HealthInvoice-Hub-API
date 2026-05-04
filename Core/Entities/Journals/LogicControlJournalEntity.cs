namespace HealthInvoice.Core.Entities.Journals;

public class LogicControlJournalEntity
{
    public int Uid{ get; set; }
    public int SchetUid { get; set; }
    public DateTime UploadDate { get; set; }
    public string Uploader { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string CodeMO { get; set; } = string.Empty;
    public string NSchet { get; set; } = string.Empty;
    public DateTime DSchet { get; set; }
    public int CountSdZ { get; set; }
    public int CountError { get; set; }
    public short Status { get; set; }
}
