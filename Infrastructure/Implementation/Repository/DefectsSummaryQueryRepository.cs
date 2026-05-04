using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Core.Dtos.Database.QueryResults;
using HealthInvoice.Core.Interfaces.Repository.Reports;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class DefectsSummaryQueryRepository(
    ILogger<DefectsSummaryQueryRepository> logger,
    InvoiceDbContextFactory invoiceDbContextFactory) : IDefectsSummaryQueryRepository
{
    public async Task<List<LogicControlDefectDto>> GetLkDefectsAsync(
        int schetUid, 
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var schetUidParam = new SqlParameter("@pSchet_uid", schetUid);

            return await dbContext.LogicControlDefects
                .FromSqlRaw("EXEC sp26_get_defects_mek @pSchet_uid", schetUidParam)
                .ToListAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при выполнение хранимой процедуры для журнала {JournalType}: {Message}",
                journalType,
                ex.Message);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Выполнение хранимой процедуры прервано по токену для журнала {JournalType}",
                journalType);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при выполнение хранимой процедуры для журнала {JournalType}: {ErrorType} — {Message}",
                journalType,
                ex.GetType().Name,
                ex.Message);

            throw;
        }
    }

    public async Task<List<FormatControlDefectEntity>> GetFkDefectsAsync(
        string sourceArchiveFilename, 
        JournalType journalType, 
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var formatControlJournalRecord = await dbContext.FormatControlJournalRecords
                .FirstOrDefaultAsync(
                    x => x.SourceArchiveFilename == sourceArchiveFilename, 
                    cancellationToken) ?? throw new InvoiceInFormatControlJournalNotFound(sourceArchiveFilename);

            return await dbContext.FormatControlDefects
                .Where(x => x.JournalUid == formatControlJournalRecord.Uid)
                .ToListAsync(cancellationToken);                
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при выполнение хранимой процедуры для журнала {JournalType}: {Message}",
                journalType,
                ex.Message);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Выполнение хранимой процедуры прервано по токену для журнала {JournalType}",
                journalType);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при выполнение хранимой процедуры для журнала {JournalType}: {ErrorType} — {Message}",
                journalType,
                ex.GetType().Name,
                ex.Message);

            throw;
        }
    }
}