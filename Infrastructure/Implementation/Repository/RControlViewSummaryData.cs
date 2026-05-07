using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Common;
using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Core.Dtos.Rcontrol.Tables;
using HealthInvoice.Core.Dtos.Rcontrol.Filters;
using HealthInvoice.Core.Interfaces.Repository.Rcontrol;
using HealthInvoice.Core.Dtos.Rcontrol.Categories;
using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class RControlViewSummaryData(
    ILogger<RControlViewSummaryData> logger,
    InvoiceDbContextFactory dbContextFactory) : IRControlViewSummaryData
{
    public async Task<List<MedOrganizationDto>> GetMedOrgsAsync(
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create(journalType);

        try
        {
            return await dbContext.MedOrganizations
                .FromSqlRaw("EXEC sp26_get_mo")
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

    public async Task<List<BillingPeriodDto>> GetPeriodsAsync(
        string codeMo,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create(journalType);
        var codeMoParam = new SqlParameter("code_mo", codeMo);

        try
        {
            return await dbContext.BillingPeriods
                 .FromSqlRaw("EXEC sp26_get_period @code_mo", codeMoParam)
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

    public async Task<List<InvoiceOverallDto>> GetInvoicesShortlyRecordsAsync(
        string codeMo,
        int Year,
        byte Month,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create(journalType);

        try
        {
            var codeMoParam = new SqlParameter("code_mo", codeMo);
            var yearParam = new SqlParameter("year", Year);
            var monthParam = new SqlParameter("month", Month);

            return await dbContext.InvoiceOverallDtos
                .FromSqlRaw("EXEC sp26_get_nschet @code_mo, @year, @month", codeMoParam, yearParam, monthParam)
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

    public async Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(int schetUid, JournalType journalType)
    {
        var dbContext = dbContextFactory.Create(journalType);

        try
        {
            var records = await dbContext.InvoiceSummary
                 .FromSqlRaw("EXEC sp26_get_z_slsvod @schet_uid", new SqlParameter("@schet_uid", schetUid))
                 .ToListAsync();

            return records.FirstOrDefault() ?? new InvoiceSummaryDto();
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

    public async Task<List<FinishedCaseDto>> GetFinishedCasesAsync(int schetUid, JournalType journalType)
    {
        var dbContext = dbContextFactory.Create(journalType);

        try
        {
            return await dbContext.FinishedCases
                .FromSqlRaw("EXEC sp26_get_z_sl_data @schet_uid", new SqlParameter("@schet_uid", schetUid))
                .ToListAsync();
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

    public async Task<List<SlEntity>> GetCasesAsync(int zSlUid, JournalType journalType)
    {
        var dbContext = dbContextFactory.Create(journalType);

        try
        {
            return await dbContext.Cases
                .Where(caseEntity => caseEntity.ZSlUid == zSlUid)
                .ToListAsync();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке получить записи из таблицы sluch для журнала {JournalType}: {Message}",
                journalType,
                ex.Message);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Получение записей из таблицы sluch прервано по токену для журнала {JournalType}",
                journalType);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при попытке получить записи из таблицы sluch для журнала {JournalType}: {ErrorType} — {Message}",
                journalType,
                ex.GetType().Name,
                ex.Message);

            throw;
        }
    }
}
