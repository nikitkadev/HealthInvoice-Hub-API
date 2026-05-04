using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Common;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class LkJournalRepository(
    ILogger<LkJournalRepository> logger,
    InvoiceDbContextFactory invoiceDbContextFactory) : ILkJournalRepository
{
    public async Task<(List<LogicControlJournalEntity>, int)> GetRecordsAsync(
        string organizationCode,
        int skip,
        int take,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(organizationCode))
            throw new ArgumentNullException(nameof(organizationCode));

        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var query = organizationCode == OrganizationConstants.AdminOrgCode
                ? dbContext.LogicControlJournalRecords.OrderByDescending(order => order.UploadDate)
                : dbContext.LogicControlJournalRecords
                    .Where(record => record.CodeMO == organizationCode)
                    .OrderByDescending(order => order.UploadDate);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .AsNoTracking()
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

    public async Task UpdateInvoiceStatusAsync(
        List<int> schetUids,
        short newStatus,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            await dbContext.LogicControlJournalRecords
                .Where(x => schetUids.Contains(x.SchetUid))
                .ExecuteUpdateAsync(
                    setter => setter.SetProperty(
                        x => x.Status,
                        newStatus),
                    cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке обновить статусы в таблице journal_flk для счетов {schetUids}: {Message}",
                string.Join(',', schetUids),
                ex.Message);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogError(
                "Операция обновления статусов таблицы journal_flk отменена");

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при попытка обновить статусы для счетов {schetUids}: {Message}",
                string.Join(',', schetUids),
                ex.Message);

            throw;
        }
    }

    public async Task<short?> GetStatusByFilenameAsync(
        string filename, 
        JournalType journalType, 
        CancellationToken cancellationToken = default)
    {
        var dbContext = invoiceDbContextFactory.Create(journalType);

        var statusParam = new SqlParameter()
        {
            Direction = ParameterDirection.Output,
            DbType = DbType.Int16,
            ParameterName = "@pRetCode"
        };

        var archiveFilenameParam = new SqlParameter()
        {
            Direction = ParameterDirection.Input,
            DbType = DbType.String,
            Size = 255,
            ParameterName = "@pFileZip",
            Value = filename
        };

        try
        {
            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp26_filenamezip_check @pRetCode OUTPUT, @pFileZip",
                [
                    archiveFilenameParam, 
                    statusParam
                ],
                cancellationToken: cancellationToken);

            return statusParam.Value != DBNull.Value ? (short)statusParam.Value : null;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
               ex,
               "Ошибка при попытке получить статус в таблице journal_flk для файла {Filename}: {Message}",
               filename,
               ex.Message);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при попытка получить статус для файла {Filename}: {Message}",
                filename,
                ex.Message);

            throw;
        }
    }
}