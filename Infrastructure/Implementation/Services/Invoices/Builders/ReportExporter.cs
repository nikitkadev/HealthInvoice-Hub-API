using System.IO.Compression;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Files;
using HealthInvoice.Core.Dtos.Reports;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Dtos.Database.QueryResults;
using HealthInvoice.Core.Interfaces.Repository.Reports;
using HealthInvoice.Core.Interfaces.Services.Invoices.Exporter;
using HealthInvoice.Core.Interfaces.Services.Invoices.Mapping;
using HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;

namespace HealthInvoice.Infrastructure.Implementation.Services.Invoices.Builders;

public class ReportExporter(
    ILogger<ReportExporter> logger,
    IDefectsSummaryQueryRepository defectsSummaryQueryRepository,
    IMapper<List<LogicControlDefectDto>, ReportFormatVDto> vReportMapper,
    IMapper<List<FormatControlDefectEntity>, ReportFormatFDto> fReportMapper,
    IXmlParser<ReportFormatFDto> fParser,
    IXmlParser<ReportFormatVDto> vParser) : IReportExporter
{
    public async Task<MemoryStream> CreateFormatValidationReportAsync(
        string sourceArchiveFilename,
        FormatControlFileNamesResult names,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var defects = await defectsSummaryQueryRepository.GetFkDefectsAsync(
                sourceArchiveFilename, 
                journalType,
                cancellationToken);

            var hInvocieDefects = defects.Where(defect => defect.FormatType == (byte)InvoiceFormatType.H).ToList();
            var lInvocieDefects = defects.Where(defect => defect.FormatType == (byte)InvoiceFormatType.L).ToList();

            var hReportModel = fReportMapper.MapTo(hInvocieDefects);

            hReportModel.FName = names.HTagFilename;
            hReportModel.FName1 = names.HOriginalFilename;

            var lReportModel = fReportMapper.MapTo(lInvocieDefects);

            lReportModel.FName = names.LTagFilename;
            lReportModel.FName1 = names.LOriginalFilename;

            var hReportXmlStream = await fParser.ParseToXmlAsync(hReportModel);
            var lReportXmlStream = await fParser.ParseToXmlAsync(lReportModel);

            var archiveMs = new MemoryStream();

            await using(var archiveStream = new ZipArchive(archiveMs, ZipArchiveMode.Create, true))
            {
                var hEntry = archiveStream.CreateEntry(names.HCallbackFilename);
                await using (var hEntryStream = await hEntry.OpenAsync(cancellationToken))
                {
                    await hReportXmlStream.CopyToAsync(hEntryStream, cancellationToken);
                }

                var lEntry = archiveStream.CreateEntry(names.LCallbackFilename);
                await using (var lEntryStream = await lEntry.OpenAsync(cancellationToken))
                {
                    await lReportXmlStream.CopyToAsync(lEntryStream, cancellationToken);
                }
            }

            archiveMs.Position = 0;
            return archiveMs;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(
                ex,
                "Операция создания ZIP-архива с отчетом о форматном контроле отменена");

            throw;
        }
    }

    public async Task<MemoryStream> CreateLogicalValidationReportAsync(
        int schetUid,
        LogicControlFileNamesResult names,
        JournalType journalType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var defects = await defectsSummaryQueryRepository.GetLkDefectsAsync(
                schetUid, 
                journalType, 
                cancellationToken);

            var fReportStream = new MemoryStream();

            if (defects.Count == 0)
            {
                var fReportModel = new ReportFormatFDto()
                {
                    FName = names.TagFilename,
                    FName1 = names.AdditionalTagFilename,

                    PRs = [new() {
                        Comment = "Без ошибок",
                        Oshib = 0
                    }]
                };

                fReportStream = await fParser.ParseToXmlAsync(fReportModel);
            }
            else
            {
                var vReportModel = vReportMapper.MapTo(defects);

                vReportModel.FName = names.TagFilename;
                vReportModel.FName1 = names.AdditionalTagFilename;

                fReportStream = await vParser.ParseToXmlAsync(vReportModel);
            }

            var archiveMs = new MemoryStream();
            await using (var archive = new ZipArchive(archiveMs, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(names.XmlFilename);
                await using (var entryStream = await entry.OpenAsync(cancellationToken))
                {
                    await fReportStream.CopyToAsync(entryStream, cancellationToken);
                };
            };

            archiveMs.Position = 0;
            return archiveMs;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(
                ex,
                "Операция создания ZIP-архива с отчетом о МЭК отменена");

            throw;
        }
    }
}
