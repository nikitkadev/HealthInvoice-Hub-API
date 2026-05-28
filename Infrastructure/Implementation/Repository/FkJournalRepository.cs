using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Dtos.Service;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class FkJournalRepository(
    ILogger<FkJournalRepository> logger,
    InvoiceDbContextFactory invoiceDbContextFactory) : IFkJournalRepository
{
    public async Task UpsertJournalFkRecordsAsync(
        List<FormatControlJournalEntity> records, 
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var filenames = records
                    .Select(record => record.SourceArchiveFilename)
                    .Distinct()
                    .ToList();

                var oldRecords = dbContext.FormatControlJournalRecords
                    .Where(record => filenames.Contains(record.SourceArchiveFilename));

                dbContext.FormatControlJournalRecords.RemoveRange(oldRecords);
                await dbContext.SaveChangesAsync(cancellationToken);

                await dbContext.FormatControlJournalRecords.AddRangeAsync(records, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(
                    ex,
                    "Произошла ошибка при попытке записать данные в таблицу journal_fk");

                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex,
                "Операция записи в JOURNAL_FK была отменена (токен отмены сработал)");

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Неожиданная ошибка при записи в JOURNAL_FK: {Message}", ex.Message);

            throw;
        }
    }

    public async Task<(List<FormatControlJournalEntity>, int)> GetRecordsAsync(
        string organizationCode,
        int skip,
        int take,
        JournalFilters filters,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(organizationCode))
            throw new ArgumentNullException(nameof(organizationCode));

        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var query = organizationCode == OrganizationConstants.AdminOrgCode
                ? dbContext.FormatControlJournalRecords
                : dbContext.FormatControlJournalRecords
                    .Where(record => record.OrganizationCode == organizationCode);

            query = FilterQuery(query, filters);

            query = query.OrderByDescending(x => x.UploadDate);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке получить записи из таблицы journal_flk по коду организации {codeOrg}",
                organizationCode);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogError(
                "Операция возврата записей из таблицы journal_flk отменена");

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при попытка получить записи о счетах для организации {codeOrg}: {Message}",
                organizationCode,
                ex.Message);

            throw;
        }
    }

    private static IQueryable<FormatControlJournalEntity> FilterQuery(
        IQueryable<FormatControlJournalEntity>? query,
        JournalFilters filters)
    {
        if (query is null)
        {
            throw new Exception();
        }

        if (string.IsNullOrEmpty(filters.GlobalFilterTarget))
        {
            return query;
        }

        var targetFilter = filters.GlobalFilterTarget;

        return query.Where(x =>
            x.SourceArchiveFilename == targetFilter ||
            x.OrganizationCode == targetFilter);
    }
}