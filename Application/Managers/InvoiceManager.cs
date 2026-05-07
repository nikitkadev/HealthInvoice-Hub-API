using System.IO.Compression;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Common;
using HealthInvoice.Core.Interfaces.Managers;
using HealthInvoice.Core.Interfaces.Services.Files;
using HealthInvoice.Core.Interfaces.Repository.Invoices;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Validators;
using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Dtos.Files;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;
using System.Reflection;

namespace HealthInvoice.Application.Managers;

public class InvoiceManager(
    ILogger<InvoiceManager> logger,
    IXmlParser<ZlListEntity> zlParser,
    IXmlParser<PersListEntity> persParser,
    IFileStorageService fileStorage,
    IFkJournalRepository fkJournalRepository,
    IFilePathService pathService,
    IInvoiceValidator invoiceValidator,
    IInvoiceRepository invoiceRepository) : IInvoiceManager
{
    public async Task RegisterInvoiceAsync(
        string filePath,
        string uploader,
        JournalType journalType,
        CancellationToken cancellationToken)
    {
        var invoice = await CreateInvoiceModelFromZipAsync(filePath, uploader, cancellationToken);
        var normalizeResult = invoice.Normalize();

        if (!normalizeResult.IsSuccess)
        {
            logger.LogWarning(
                "Не удалось нормализовать счет по пути {filePath}",
                filePath);

            return;
        }

        await invoiceRepository.InsertInvoiceAsync(invoice.Normalize().Value, journalType, cancellationToken);
    }

    public async Task UpdateInvoiceAsync(
        string filePath,
        string uploader,
        int schetUid,
        JournalType journalType,
        CancellationToken cancellationToken)
    {
        var invoice = await CreateInvoiceModelFromZipAsync(filePath, uploader, cancellationToken);
        var normalizeResult = invoice.Normalize();

        if (!normalizeResult.IsSuccess)
        {
            logger.LogWarning(
                "Не удалось нормализовать счет по пути {filePath}",
                filePath);

            return;
        }

        await invoiceRepository.RewriteInvoiceAsync(schetUid, normalizeResult.Value, journalType,cancellationToken);
    }
    
    public async Task<List<InvoiceFormatControlResponse>> ValidateInvoiceStructureAsync(
        InvoiceValidationParameters invoices,
        CancellationToken cancellationToken)
    {
        var summaryResult = new List<InvoiceStructureValidationResult>();
        var fkJournalRecords = new List<FormatControlJournalEntity>();

        try
        {
            foreach (var item in invoices.Archives)
            {
                var invoiceSummaryResult = new InvoiceStructureValidationResult()
                {
                    ArchiveFilename = item.ArchiveFilename,
                    ArchiveFileSize = item.ArchiveStream.Length,
                    ComplianceResult = new(),
                    IntegrityResult = new()
                };

                using var buffer = new MemoryStream();

                await item.ArchiveStream.CopyToAsync(buffer, cancellationToken);

                var integrityValidationResult = new IntegrityValidationResult();
                var complianceValidationResult = new ComplianceValidationResult();

                integrityValidationResult = await invoiceValidator.ValidateFileMetadata(
                    buffer, 
                    item.ArchiveFilename, 
                    invoices.JournalType, 
                    item.OrganizationCode);

                buffer.Position = 0;

                if (!integrityValidationResult.IsValid)
                {
                    invoiceSummaryResult.IntegrityResult = integrityValidationResult;
                    summaryResult.Add(invoiceSummaryResult);

                    continue;
                }

                complianceValidationResult = await invoiceValidator.ValidateInvoiceFormatAsync(
                    buffer, 
                    item.ArchiveFilename, 
                    invoices.JournalType);


                if (complianceValidationResult.IsValid)
                {
                    buffer.Position = 0;

                    string savePath = pathService.CreateFilePath(
                        item.ArchiveFilename,
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                        [
                            "archive",
                            item.OrganizationCode,
                            "invoices"]);

                    if (string.IsNullOrEmpty(savePath))
                    {
                        logger.LogWarning(
                            "Путь, по которому должен был быть сохранен файл, не получен");

                        continue;
                    }

                    invoiceSummaryResult.ArchiveFilePath = savePath;

                    await fileStorage.StoreFileAsync(
                        new FileSaveParameters()
                        {
                            FileName = item.ArchiveFilename,
                            FileStream = buffer,
                            OrganizationCode = item.OrganizationCode,
                            SavePath = savePath
                        },
                        cancellationToken);
                }

                invoiceSummaryResult.IntegrityResult = integrityValidationResult;
                invoiceSummaryResult.ComplianceResult = complianceValidationResult;

                fkJournalRecords.Add(
                    new()
                    {
                        OrganizationCode = item.OrganizationCode,
                        SourceArchiveFilename = item.ArchiveFilename,
                        UploadDate = DateTime.Now,
                        Status = invoiceSummaryResult.IsSuccess ? (short)1 : (short)0,

                        FormatControlDeffects = !complianceValidationResult.IsValid
                            ? [..
                                complianceValidationResult.XsdValidationResult.HErrors
                                    .Select(error => new FormatControlDefectEntity()
                                    {
                                        Comment = error,
                                        FormatType = (byte)InvoiceFormatType.H
                                    }) ?? [],
                                ..complianceValidationResult.XsdValidationResult.LErrors
                                    .Select(error => new FormatControlDefectEntity()
                                    {
                                        Comment = error,
                                        FormatType = (byte)InvoiceFormatType.L
                                    }) ?? [],
                                ..complianceValidationResult.PacIdsValidationResult?.Errors?
                                    .Select(error => new FormatControlDefectEntity()
                                    {
                                        Comment = error,
                                        FormatType = (byte)InvoiceFormatType.H
                                    }) ?? [],
                                ..(!string.IsNullOrEmpty(complianceValidationResult.InvoiceWriteEligibilityResult.Error)
                                    ? (IEnumerable<FormatControlDefectEntity>)[new FormatControlDefectEntity() {
                                        Comment = complianceValidationResult.InvoiceWriteEligibilityResult.Error,
                                        FormatType = (byte)InvoiceFormatType.H
                                    }]
                                    : Array.Empty<FormatControlDefectEntity>())
                              ] : [],
                    });

                summaryResult.Add(invoiceSummaryResult);
            }

            await fkJournalRepository.UpsertJournalFkRecordsAsync(fkJournalRecords, invoices.JournalType, cancellationToken);

            return [.. summaryResult.Select(
                result => new InvoiceFormatControlResponse()
                {
                    IsSuccess = result.IsSuccess,
                    WillRewrite = result.WillRewrite,
                    FileSize = result.ArchiveFileSize,
                    UploadArchiveFilename = result.ArchiveFilename,
                    UploadArchiveFilePath = result.ArchiveFilePath,
                    ErrorMessage = result.ClientErrorMessage,
                    SchetUid = result.ComplianceResult.InvoiceWriteEligibilityResult.SchetUid
                })];
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Операция проверки формата для счета отменена");

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Неожиданная ошибка: {Message}",
                ex.Message);

            throw;
        }
    }

    private async Task<(ZlListEntity, PersListEntity)> CreateInvoiceModelFromZipAsync(
        string filePath,
        string uploader,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException(
                "filePath не может быть пустым",
                nameof(filePath));

        if (string.IsNullOrEmpty(uploader))
            throw new ArgumentException(
                "uploader не может быть пустым",
                nameof(uploader));

        try
        {
            await using var file = await fileStorage.RetrieveFileStreamAsync(filePath, cancellationToken);
            await using var archive = new ZipArchive(file, ZipArchiveMode.Read, leaveOpen: true);

            var hEntry = archive.Entries.FirstOrDefault(e => e.Name.StartsWith('H'));
            var lEntry = archive.Entries.FirstOrDefault(e => e.Name.StartsWith('L'));

            if (hEntry == null)
                throw new InvalidDataException(
                    $"В архиве не найдена запись с префиксом 'H' для файла: {filePath}");

            if (lEntry == null)
                throw new InvalidDataException(
                    $"В архиве не найдена запись с префиксом 'L' для файла: {filePath}");

            await using var hStream = await hEntry.OpenAsync(cancellationToken);
            await using var lStream = await lEntry.OpenAsync(cancellationToken);

            var hInvoiceModel = await zlParser.ParseToEntityAsync(hStream);
            var lInvoiceModel = await persParser.ParseToEntityAsync(lStream);

            if (hInvoiceModel == null)
                throw new InvalidOperationException("Не удалось распарсить H‑запись из архива");

            if (lInvoiceModel == null)
                throw new InvalidOperationException("Не удалось распарсить L‑запись из архива");

            hInvoiceModel.Uploader = uploader;

            return (hInvoiceModel, lInvoiceModel);
        }
        catch (IOException ex)
        {
            logger.LogError(
                ex,
                "IO‑ошибка при работе с файлом: {FilePath}", filePath);

            throw;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Операция создания модели счёта отменена по токену отмены: {FilePath}", filePath);

            throw;
        }
        catch (InvalidDataException ex)
        {
            logger.LogError(
                ex,
                "Некорректные данные в архиве: {Message}",
                ex.Message);

            throw;
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Ошибка бизнес‑логики при парсинге: {Message}",
                ex.Message);

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Неожиданная ошибка при создании модели счёта: {FilePath}",
                filePath);

            throw;
        }
    }
}