using HealthInvoice.Core.Dtos.Database.QueryResults;

namespace HealthInvoice.Core.Entities.Journals;

public class FormatControlJournalEntity
{
    public int Uid { get; set; }
    public string SourceArchiveFilename { get; set; } = string.Empty;
    public string OrganizationCode { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }  
    public short Status { get; set; }

    public List<FormatControlDefectEntity> FormatControlDeffects { get; set; } = [];
}
