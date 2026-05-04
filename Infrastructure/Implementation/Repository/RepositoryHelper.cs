using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Core.Interfaces.Repository.Helpers;
using HealthInvoice.Core.Dtos.Files;
using HealthInvoice.Core.Common;

namespace HealthInvoice.Infrastructure.Implementation.Repository;

public class RepositoryHelper(
    ILogger<RepositoryHelper> logger,
    InvoiceDbContextFactory invoiceDbContextFactory) : IRepositoryHelper
{
    public async Task<LogicControlFileNamesResult> GetFileNamesForControlReportAsync(
        int schetUid,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var schetUidParam =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "@pSchet_uid",
                    Value = schetUid
                };

            var archiveFilenameParam =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = 26,
                    ParameterName = "@Filename_zip"
                };

            var xmlFilenameParam =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = 26,
                    ParameterName = "@Filename_xml"
                };

            var tagFilenameParam =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = 26,
                    ParameterName = "@Tag_Filename"
                };

            var tagFilename_1Param =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = 26,
                    ParameterName = "@Tag_Filename1"
                };


            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp26_get_defects_mek_filename " +
                "@pSchet_uid, " +
                "@Filename_zip OUTPUT, " +
                "@Filename_xml OUTPUT, " +
                "@Tag_Filename OUTPUT, " +
                "@Tag_Filename1 OUTPUT",
                [schetUidParam,
                archiveFilenameParam,
                xmlFilenameParam,
                tagFilenameParam,
                tagFilename_1Param],
                cancellationToken);

            return
                new LogicControlFileNamesResult()
                {
                    ArchiveFilename = archiveFilenameParam.Value == DBNull.Value ? string.Empty : (string)archiveFilenameParam.Value,
                    XmlFilename = xmlFilenameParam.Value == DBNull.Value ? string.Empty : (string)xmlFilenameParam.Value,
                    TagFilename = tagFilenameParam.Value == DBNull.Value ? string.Empty : (string)tagFilenameParam.Value,
                    AdditionalTagFilename = tagFilename_1Param.Value == DBNull.Value ? string.Empty : (string)tagFilename_1Param.Value
                };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при получение имен файлов для ZIP-отчета.");

            throw;
        }
    }

    public async Task<FormatControlFileNamesResult> GetFileNamesForFormatReportAsync(
        string sourceFilename,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = invoiceDbContextFactory.Create(journalType);

        try
        {
            var sourceArchiveFilenameParam =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Input,
                    ParameterName = "@pFileZip",
                    Value = sourceFilename,
                    Size = 255
                };


            var resultArchiveFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    ParameterName = "@pfile_FT",
                    Size = 255
                };


            var sourceHInvoiceFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_HM"
                };

            var HTagFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_HT"
                };

            var resultHFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_HT_ve"
                };

            var sourceLInvoiceFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_LM"
                };

            var LTagFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_LT"
                };

            var resultLFilename =
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Output,
                    Size = 255,
                    ParameterName = "@pfile_LT_ve"
                };

            await dbContext.Database.ExecuteSqlRawAsync("" +
                "EXEC sp26_filenamezip_genshin " +
                "@pfile_FT OUTPUT, " +
                "@pfile_HM OUTPUT," +
                "@pfile_HT OUTPUT, " +
                "@pfile_LM OUTPUT, " +
                "@pfile_LT OUTPUT, " +
                "@pfile_LT_ve OUTPUT, " +
                "@pfile_HT_ve OUTPUT, " +
                "@pFileZip",
                [resultArchiveFilename, sourceHInvoiceFilename, HTagFilename, sourceLInvoiceFilename, LTagFilename, resultHFilename, resultLFilename, sourceArchiveFilenameParam],
                cancellationToken: cancellationToken);

            return
                new FormatControlFileNamesResult()
                {
                    ArchiveFilename = resultArchiveFilename.Value.ToString() ?? string.Empty,

                    HOriginalFilename = sourceHInvoiceFilename.Value.ToString() ?? string.Empty,
                    HTagFilename = HTagFilename.Value.ToString() ?? string.Empty,
                    HCallbackFilename = resultHFilename.Value.ToString() ?? string.Empty,

                    LOriginalFilename = sourceLInvoiceFilename.Value.ToString() ?? string.Empty,
                    LTagFilename = LTagFilename.Value.ToString() ?? string.Empty,
                    LCallbackFilename = resultLFilename.Value.ToString() ?? string.Empty,
                };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при получение имен файлов для ZIP-отчета.");

            throw;
        }
    }
}