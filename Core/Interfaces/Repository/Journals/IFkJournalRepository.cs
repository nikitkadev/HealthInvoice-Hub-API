using HealthInvoice.Core.Common;
using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Core.Interfaces.Repository.Journals;

public interface IFkJournalRepository
{
    /// <summary>
    /// Заполняет таблицу journal_fk со всеми дефектами.
    /// </summary>
    /// <param name="records">Список сущностей для записи.</param>
    public Task UpsertJournalFkRecordsAsync(
        List<FormatControlJournalEntity> records, 
        JournalType journalType, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationCode"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="journalType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(List<FormatControlJournalEntity>, int)> GetRecordsAsync(
        string organizationCode,
        int skip,
        int take,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);
}
