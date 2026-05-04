namespace HealthInvoice.Core.Entities.Journals;

public class FormatControlDefectEntity
{
    public int Uid { get; set; }
    public int JournalUid { get; set; }
    public byte FormatType { get; set; }
    public string Comment { get; set; } = string.Empty;
}
