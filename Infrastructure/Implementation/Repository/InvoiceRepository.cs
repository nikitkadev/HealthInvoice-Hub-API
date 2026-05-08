using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Interfaces.Repository.Invoices;
using HealthInvoice.Infrastructure.Database.EF.Context;
using HealthInvoice.Infrastructure.Factories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class InvoiceRepository(
    ILogger<InvoiceRepository> logger,
    InvoiceDbContextFactory dbContextFactory) : IInvoiceRepository
{
    public async Task InsertInvoiceAsync(
        (ZlListEntity, PersListEntity) models,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var insertInvoiceDbContext = dbContextFactory.Create(journalType);
        await using var invoiceTransaction = await insertInvoiceDbContext.Database.BeginTransactionAsync(cancellationToken);

        int _schetUid;

        try
        {
            await insertInvoiceDbContext.ZlLists.AddAsync(models.Item1, cancellationToken);
            await insertInvoiceDbContext.PersLists.AddAsync(models.Item2, cancellationToken);

            await insertInvoiceDbContext.SaveChangesAsync(cancellationToken);
            insertInvoiceDbContext.ChangeTracker.Clear();

            _schetUid = models.Item1.Schet!.Uid;

            await invoiceTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при вставке счёта для журнала {JournalType}: {ErrorType} — {Message}",
                journalType,
                ex.GetType().Name,
                ex.Message);

            await invoiceTransaction.RollbackAsync(cancellationToken);
            throw;
        }

        await using var updateJournalFlkDbContext = dbContextFactory.Create(journalType);
        await using var journalUpdateTransaction = await updateJournalFlkDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await updateJournalFlkDbContext.LogicControlJournalRecords
                .AddAsync(
                    new LogicControlJournalEntity
                    {
                        CodeMO = models.Item1.Schet!.CodeMO,
                        CountError = 0,
                        CountSdZ = 0,
                        FileName = models.Item1.Zglv!.FileName!,
                        SchetUid = _schetUid,
                        UploadDate = models.Item1.UploadDate,
                        Uploader = models.Item1.Uploader ?? string.Empty,
                        NSchet = models.Item1.Schet.Nschet,
                        DSchet = models.Item1.Schet.Dschet,
                        Status = 2

                    },
                    cancellationToken);

            await updateJournalFlkDbContext.SaveChangesAsync(cancellationToken);
            await journalUpdateTransaction.CommitAsync(cancellationToken);
            updateJournalFlkDbContext.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при обновление журнала ФЛК для типа журнала {JournalType}: {ErrorType} — {Message}",
                journalType,
                ex.GetType().Name,
                ex.Message);

            await journalUpdateTransaction.RollbackAsync(cancellationToken);

            throw;
        }
    }

    public async Task RemoveInvoiceAsync(
        int schetUid,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = dbContextFactory.Create(journalType);
        await using var removingTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            dbContext.Database.SetCommandTimeout(new TimeSpan(0, 10, 0));

            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[sp26_delbyschetuid] @pSchet_uid",
                new SqlParameter("@pSchet_uid", SqlDbType.Int)
                {
                    Value = schetUid
                });

            await removingTransaction.CommitAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при выполнение хранимой процедуры для журнала {JournalType}: {Message}",
                journalType,
                ex.Message);

            await removingTransaction.RollbackAsync(cancellationToken);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Выполнение хранимой процедуры прервано по токену для журнала {JournalType}",
                journalType);

            await removingTransaction.RollbackAsync(cancellationToken);

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

            await removingTransaction.RollbackAsync(cancellationToken);

            throw;
        }
    }

    public async Task RewriteInvoiceAsync(
        int schetUid,
        (ZlListEntity, PersListEntity) models,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        int _retryCount = 0;

        while (_retryCount < ServiceConstants.MaxRetries)
        {
            if (models.Item1 == null || models.Item2 == null)
                throw new ArgumentNullException(
                    models.Item1 == null ? nameof(models.Item1) : nameof(models.Item2),
                    "Модели для вставки не могут быть null");

            await using var dbContext = dbContextFactory.Create(journalType);
            await using var rewriteTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await dbContext.Database.ExecuteSqlRawAsync(
                     "EXEC [dbo].[sp26_delbyschetuid] @pSchet_uid",
                     new SqlParameter("@pSchet_uid", SqlDbType.Int)
                     {
                         Value = schetUid
                     });

                await dbContext.ZlLists.AddAsync(models.Item1, cancellationToken);
                await dbContext.PersLists.AddAsync(models.Item2, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                await dbContext.LogicControlJournalRecords.AddAsync(
                    new LogicControlJournalEntity
                    {
                        CodeMO = models.Item1.Schet!.CodeMO,
                        CountError = 0,
                        CountSdZ = 0,
                        FileName = models.Item1.Zglv!.FileName!,
                        SchetUid = models.Item1.Schet.Uid,
                        UploadDate = models.Item1.UploadDate,
                        Uploader = models.Item1.Uploader ?? string.Empty,
                        NSchet = models.Item1.Schet.Nschet,
                        DSchet = models.Item1.Schet.Dschet,
                        Status = 2

                    }, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
                await rewriteTransaction.CommitAsync(cancellationToken);

                return;
            }
            catch (SqlException ex) when (ex.Number == CodeConstants.SqlExceptionDeadlockCode || ex.Number == CodeConstants.SqlExceptionTimeoutCode)
            {
                await rewriteTransaction.RollbackAsync(cancellationToken);

                _retryCount++;

                logger.LogError(
                    ex,
                    "[HealthInvoice Hub] Дедлок при перезаписи счета {SchetUid}. Попытка {RetryCount}/{MaxRetries}",
                    schetUid,
                    _retryCount,
                    ServiceConstants.MaxRetries);

                if (_retryCount >= ServiceConstants.MaxRetries)
                {
                    logger.LogError(
                        "[HealthInvoice Hub] Не удалось перезаписать счёт {SchetUid} после {MaxRetries} попыток",
                        schetUid,
                        ServiceConstants.MaxRetries);

                    throw;
                }

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, _retryCount - 1) + Random.Shared.NextDouble()), cancellationToken);
            }
            catch (Exception ex)
            {
                await rewriteTransaction.RollbackAsync(cancellationToken);
                await dbContext.DisposeAsync();

                logger.LogError(ex,
                    "[HealthInvoice Hub] Критическая ошибка при перезаписи счёта {SchetUid}",
                    schetUid);

                throw;
            }
        }
    }

    public async Task PerformLogicControlAsync(
        int schetUid,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default)
    {
        int _retryCount = 0;

        try
        {
            while (_retryCount < ServiceConstants.MaxRetries)
            {
                try
                {
                    await using var dbContext = dbContextFactory.Create(journalType);

                    dbContext.Database.SetCommandTimeout(new TimeSpan(1, 0, 0));

                    await dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[sp26_check_preparation] @pSchet_uid",
                        new SqlParameter("@pSchet_uid", SqlDbType.Int)
                        {
                            Value = schetUid
                        });

                    return;
                }
                catch (SqlException ex) when (ex.Number == CodeConstants.SqlExceptionDeadlockCode)
                {
                    _retryCount++;

                    logger.LogWarning(
                        "Дедлок при проверке МЭК для счета {SchetUid}, попытка {RetryCount}/{MaxRetries}",
                        schetUid,
                        _retryCount,
                        ServiceConstants.MaxRetries);

                    if (_retryCount >= ServiceConstants.MaxRetries)
                        throw;

                    await Task.Delay(
                        TimeSpan.FromSeconds(Math.Pow(2, _retryCount - 1)),
                        cancellationToken);
                }
            }
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

    public async Task<InvoiceCheckOutput> CheckInvoiceExistenceAsync(
        InvoiceCheckParameters request,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default)
    {
        var h_filename = new SqlParameter("@pFileH", request.HInvoiceFilename.Split(".")[0]);
        var l_filename = new SqlParameter("@pFileL", request.LInvoiceFilename.Split(".")[0]);
        var l_invoiceFilename = new SqlParameter("@pFileL_Filename", request.LInvoiceEntity.PZglv!.FileName!);
        var l_invoiceFilename1 = new SqlParameter("@pFileL_Filename1", request.LInvoiceEntity.PZglv!.FileName1!);
        var h_invoiceFilename = new SqlParameter("@pFileH_Filename", request.HInvoiceEntity.Zglv!.FileName!);
        var code = new SqlParameter("@pCode", request.HInvoiceEntity.Schet!.Code);
        var codeMo = new SqlParameter("@pCode_MO", request.HInvoiceEntity.Schet.CodeMO);
        var year = new SqlParameter("@pYear", request.HInvoiceEntity.Schet.Year);
        var month = new SqlParameter("@pMonth", request.HInvoiceEntity.Schet.Month);
        var nSchet = new SqlParameter("@pNSchet", request.HInvoiceEntity.Schet.Nschet);
        var plat = new SqlParameter("@pPlat", request.HInvoiceEntity.Schet.Plat);

        var retCode = new SqlParameter("@pRetCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var retMessage = new SqlParameter("@pRetMessage", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
        var schetUid = new SqlParameter("@pSchet_uid", SqlDbType.Int) { Direction = ParameterDirection.Output };

        await using var dbContext = dbContextFactory.Create(journalType);

        try
        {
            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp26_filename_check " +
                "@pRetCode OUTPUT, @pRetMessage OUTPUT, @pSchet_uid OUTPUT, " +
                "@pFileH, @pFileL, @pFileL_Filename, @pFileL_Filename1, @pFileH_Filename, " +
                "@pCode, @pCode_MO, @pYear, @pMonth, @pNSchet, @pPlat",
                parameters: [retCode, retMessage, schetUid, h_filename, l_filename, l_invoiceFilename,
                l_invoiceFilename1, h_invoiceFilename, code, codeMo, year, month, nSchet, plat]);

            return
                new InvoiceCheckOutput
                {
                    Code = retCode.Value == DBNull.Value ? -1 : (int)retCode.Value,
                    Message = retMessage.Value?.ToString() ?? string.Empty,
                    SchetUid = schetUid.Value == DBNull.Value ? null : (int)schetUid.Value
                };
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