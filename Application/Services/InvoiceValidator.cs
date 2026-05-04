using System.Xml;
using System.IO.Compression;

using Microsoft.Extensions.Logging;

using HealthInvoice.Core.Interfaces.Repository.Invoices;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Extractors;
using HealthInvoice.Core.Interfaces.Services.Invoices.Management;
using HealthInvoice.Core.Interfaces.Services.Invoices.Validators;
using HealthInvoice.Core.Dtos.Invoices;
using HealthInvoice.Core.Common;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Application.Services;

public class InvoiceValidator(
    ILogger<InvoiceValidator> logger,
    ISchemaService schemaService,
    IPacIdsExtractor extractor,
    IInvoiceRepository invoiceRepository,
    ILkJournalRepository journalRepository,
    IXmlParser<ZlListEntity> h_Parser,
    IXmlParser<PersListEntity> l_Parser) : IInvoiceValidator
{
    public async Task<IntegrityValidationResult> ValidateFileMetadata(
        Stream fileStream,
        string uploadFilename,
        JournalType journalType,
        string organizationCode)
    {
        var invoiceMetaValidationResult = new IntegrityValidationResult() { IsValid = true };

        try
        {
            if (fileStream is null)
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    $"Непредвиденная ошибка при проверке выгружаемого файла. " +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Переданный файл с именем {UploadFilename} оказался null. " +
                    "Уникальный идентификатор ошибки: {Guid}",
                    uploadFilename,
                    guid);

                return invoiceMetaValidationResult;
            }

            if (fileStream.Length > FileConstants.FileSizeLimit)
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    $"Размер файла ({fileStream.Length} байт) превышает лимит " +
                    $"({FileConstants.FileSizeLimit} байт)\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Переданный файл {UploadFilename} превышает установленный размер в {MaxLimitFileSize}.",
                    uploadFilename,
                    FileConstants.FileSizeLimit);

                return invoiceMetaValidationResult;
            }

            if (string.IsNullOrEmpty(uploadFilename))
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    "Непредвиденная ошибка при проверке ZIP-архива.\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Имя переданного ZIP-архива оказалось null. " +
                    "Уникальный идентификатор ошибки: {Guid}",
                    guid);

                return invoiceMetaValidationResult;
            }

            if (!uploadFilename.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    $"Файл имеет разрешение, отличное от ZIP.\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Переданный файл {UploadFilename} не является файлом архива и имеет разрешение отличное от .ZIP. " +
                    "Уникальный идентификатор ошибки: {Guid}",
                    uploadFilename,
                    guid);

                return invoiceMetaValidationResult;
            }

            var archiveNameValidateResult = uploadFilename.HasCorrectArchiveName(organizationCode, journalType);

            if (archiveNameValidateResult.IsFailure)
            {
                if (!string.IsNullOrEmpty(archiveNameValidateResult.ErrorMessage))
                {
                    invoiceMetaValidationResult.FillError(archiveNameValidateResult.ErrorMessage);

                    logger.LogError(
                        "Ошибка проверки имени архива. Сообщение: {ErrorMessage}. " +
                        "Уникальный идентификатор ошибки: {Guid}",
                        archiveNameValidateResult.ErrorMessage,
                        archiveNameValidateResult.ErrorId);

                    return invoiceMetaValidationResult;
                }
            }

            if (fileStream.Length == 0)
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    $"Переданный ZIP-архив пуст.\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Переданный ZIP-архив с именем {ArchiveFilename} оказался пуст. " +
                    "Уникальный идентификатор ошибки: {Guid}",
                    uploadFilename,
                    guid);

                return invoiceMetaValidationResult;
            }

            if (!fileStream.CanRead)
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    $"Переданный ZIP-архив поврежден.\n" +
                    $"Невозможно открыть и прочитать содержимое.\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Переданный ZIP-архив с именем {ArchiveFilename} поврежден. Невозможно открыть и прочитать содержимое. " +
                    "Уникальный идентификатор ошибки: {Guid}",
                    uploadFilename,
                    guid);

                return invoiceMetaValidationResult;
            }

            var logicControlStatus = await journalRepository.GetStatusByFilenameAsync(
                uploadFilename,
                journalType,
                CancellationToken.None);

            if (logicControlStatus is not null && (MECStatus)logicControlStatus == MECStatus.Processing)
            {
                var guid = Guid.NewGuid();

                invoiceMetaValidationResult.FillError(
                    "Переданный счет проходит првоерку на МЭК. " +
                    "Пожалуйста, дождитесь окончания проверки и только после этого отправьте счет на ФК!\n" +
                    $"Уникальный идентификатор ошибки: {guid}");

                logger.LogError(
                    "Пользователь закинул счет, который проходит проверку МЭК. " +
                    "Не сомневались!\n" +
                    "Имя архива: {ArchiveFilename}. " +
                    "Уникальный идентификатор ошибки: {Guild}",
                    uploadFilename,
                    guid);

                return invoiceMetaValidationResult;
            }

            try
            {
                using var archive = new ZipArchive(
                    fileStream,
                    ZipArchiveMode.Read,
                    leaveOpen: true);

                var entries = archive.Entries.ToList();

                if (entries.Count != 2)
                {
                    var guid = Guid.NewGuid();

                    invoiceMetaValidationResult.FillError(
                        "Количество файлов внутри ZIP-архива не соответствует ожидаемому количеству - 2.\n" +
                        $"Уникальный идентификатор ошибки: {guid}");

                    logger.LogError(
                        "Количество файлов в переданном архиве с именем {ArchiveFilename} не соответствует ожидаемой структуре из двух файлов. " +
                        "Уникальный идентификатор ошибки: {Guild}",
                        uploadFilename,
                        guid);

                    return invoiceMetaValidationResult;
                }

                string h_invoiceFilename = string.Empty;
                string l_invoiceFilename = string.Empty;

                foreach (var entry in entries)
                {
                    if (entry is null)
                    {
                        var guid = Guid.NewGuid();

                        invoiceMetaValidationResult.FillError(
                            "Непредвиденная ошибка при попытке прочитать файлы внутри архива.\n" +
                            $"Уникальный идентификатор ошибки: {guid}");

                        logger.LogError(
                            "Внутри архива с именем {ArchiveFilename} один из файлов является null. " +
                            "Не знаю, может ли такое быть вообще, что entry будет null. Надеюсь, мы никогда не узнаем!",
                            uploadFilename);

                        return invoiceMetaValidationResult;
                    }

                    if (string.IsNullOrEmpty(entry.FullName))
                    {
                        var guid = Guid.NewGuid();

                        invoiceMetaValidationResult.FillError(
                            "Имя файла внутри архива не имеет имени. Проверьте содержимое архива!\n" +
                            $"Уникальный идентификатор ошибка: {guid}");

                        logger.LogError(
                            "Архив с именем {ArchiveFilename} содержит внутри файл, у которого пустое имя (интересный кейс).\n" +
                            "Уникальный идентификатор ошибки: {Guid}",
                            uploadFilename,
                            guid);

                        return invoiceMetaValidationResult;
                    }

                    if (!entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        var guid = Guid.NewGuid();

                        invoiceMetaValidationResult.FillError(
                            $"Файл {entry.FullName} счета имеет формат, отличный от .xml.\n" +
                            $"Уникальный идентификатор ошибки: {guid}");

                        logger.LogError(
                            "В переданном архиве с именем {ArchiveFilename} обнаружен файл {InvoiceFilename}, который не имеет допустимого разрешения .xml.\n" +
                            "Уникальный идентификатор ошибки: {Guid}",
                            uploadFilename,
                            entry.FullName,
                            guid);

                        return invoiceMetaValidationResult;
                    }

                    if (entry.Name.StartsWith('H')) h_invoiceFilename = entry.Name;
                    if (entry.Name.StartsWith('L')) l_invoiceFilename = entry.Name;
                }

                string validateFilenameErrorMessage = ValidateFileName(
                    uploadFilename,
                    h_invoiceFilename,
                    l_invoiceFilename,
                    journalType);

                if (!string.IsNullOrEmpty(validateFilenameErrorMessage))
                {
                    var guid = Guid.NewGuid();

                    invoiceMetaValidationResult.FillError(
                        validateFilenameErrorMessage + "\n" +
                        $"Уникальный идентификатор ошибки: {guid}");

                    logger.LogError(
                        "Ошибка проверки файла счета на соответстие имени с паттерном. Текст ошибки: {ErrorMessage}\n" +
                        "Уникальный идентификатор ошибки: {Guid}",
                        validateFilenameErrorMessage,
                        guid);

                    return invoiceMetaValidationResult;
                }
            }
            catch (InvalidDataException ex)
            {
                logger.LogError(
                    ex,
                    "Ошибка при чтении ZIP-архива {FileName}.\n" +
                    "Текст ошибки: {ErrorMessage}",
                    uploadFilename,
                    ex.Message);

                throw;

            }
            catch (IOException ex)
            {
                logger.LogError(
                    ex,
                    "IO-ошибка при обработке ZIP-архива {FileName}.\n" +
                    "Текст ошибки: {ErrorMessage}",
                    uploadFilename,
                    ex.Message);

                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Неожиданная ошибка при валидации метаданных файла {FileName}.\n" +
                "Текст ошибки: {ErrorMessage}",
                uploadFilename,
                ex.Message);

            throw;
        }

        return invoiceMetaValidationResult;
    }
    public async Task<ComplianceValidationResult> ValidateInvoiceFormatAsync(
        Stream archiveStream,
        string archiveFilename,
        JournalType journalType)
    {
        var complianceValidationResult = new ComplianceValidationResult();

        try
        {
            if (archiveStream is null)
            {
                var guid = Guid.NewGuid();

                complianceValidationResult.ClientErrorMessage =
                    $"Непредвиденная ошибка при попытке форматной проверки переданного архива!\n" +
                    $"Уникальный идентификатор ошибки: {guid}";

                logger.LogError(
                    "Переданный на проверку архив оказался null.\n" +
                    "Уникальный идентификатор ошибки: {Guid}",
                    guid);

                return complianceValidationResult;
            }

            if (!archiveStream.CanRead)
            {
                var guid = Guid.NewGuid();

                complianceValidationResult.ClientErrorMessage =
                    $"Непредвиденная ошибка при попытке форматной проверки переданного архива!\n" +
                    $"Уникальный идентификатор ошибки: {guid}";

                logger.LogError(
                    "Переданный на проверку архив поврежден и недоступен для чтения.\n" +
                    "Уникальный идентификатор ошибки: {Guid}",
                    guid);

                return complianceValidationResult;
            }

            var (invoicesPair, clientErrorMessage) = await ExtractInvoiceStreamsAsync(archiveStream);
            if (invoicesPair is null)
            {
                var guid = Guid.NewGuid();

                if (!string.IsNullOrEmpty(clientErrorMessage))
                {
                    complianceValidationResult.ClientErrorMessage = clientErrorMessage + "\n" +
                    $"Уникальный идентификатор ошибки: {guid}";

                    logger.LogError(
                        "Произошла ошибка при попытке извлечь поток XML-файлов внутри переданного архива. Текст ошибки: {ErrorMessage}.\n" +
                        "Уникальный идентификтор ошибки: {Guid}",
                        clientErrorMessage,
                        guid);

                    return complianceValidationResult;
                }

                complianceValidationResult.ClientErrorMessage =
                   $"Непредвиденная ошибка при попытке форматной проверки переданного архива!\n" +
                   $"Уникальный идентификатор ошибки: {guid}";

                logger.LogError(
                    "Переданный картеж после извлечения XML-счетов из архива содержит null потоки счетов.\n" +
                    "Уникальный идентификатор ошибки: {Guid}",
                    guid);

                return complianceValidationResult;
            }

            var xsdValidationResult = await XsdValidateInvoicesAsync(
                (invoicesPair.HInvoiceStream, invoicesPair.LInvoiceStream), 
                (FileConstants.H_SchemaName, FileConstants.L_SchemaName));

            if (!xsdValidationResult.IsValid)
            {
                complianceValidationResult.XsdValidationResult = xsdValidationResult;
                return complianceValidationResult;
            }

            var invoiceWriteEligibilityResult = await CheckInvoiceInDbAsync(invoicesPair, journalType);

            if (!invoiceWriteEligibilityResult.IsValid)
            {
                complianceValidationResult.InvoiceWriteEligibilityResult = invoiceWriteEligibilityResult;
                return complianceValidationResult;
            }

            var pacsMatchingResult = await ValidatePacsMatchingAsync(invoicesPair.HInvoiceStream, invoicesPair.LInvoiceStream);

            if(!pacsMatchingResult.IsValid)
            {
                complianceValidationResult.PacIdsValidationResult = pacsMatchingResult;
                return complianceValidationResult;
            }

            complianceValidationResult.XsdValidationResult = xsdValidationResult;
            complianceValidationResult.InvoiceWriteEligibilityResult = invoiceWriteEligibilityResult;
            complianceValidationResult.PacIdsValidationResult = pacsMatchingResult;

            return complianceValidationResult;
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(
                ex,
                "Недопустимая операция при попытке проверить формат отправленного счета {ArchiveName}. Текст ошибки: {ErrorMessage}.",
                archiveFilename,
                ex.Message);

            throw;
        }
        catch (XmlException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке сверить XML-счет с XSD-схемой счета {ArchiveName}. Текст ошибки: {ErrorMessage}.",
                archiveFilename,
                ex.Message);

            throw;
            
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Критическая ошибка при попытке провести форматный контроль счета {ArchiveName}. Текст ошибки: {ErrorMessage}.",
                archiveFilename,
                ex.Message);

            throw;
        }
    }

    #region FormatPipeline
    private static async Task<(XmlInvoicesExtractionResult? invoicesPair, string errorClientMessage)> ExtractInvoiceStreamsAsync(Stream invoiceFileStream)
    {
        using var archive = new ZipArchive(
            invoiceFileStream,
            ZipArchiveMode.Read,
            leaveOpen: true);

        var hEntry = archive.Entries.FirstOrDefault(e => e.Name.StartsWith('H'));
        if (hEntry is null)
            return (null,$"XML-счет, начинающийся на 'H', не найден внутри архива");

        var lEntry = archive.Entries.FirstOrDefault(e => e.Name.StartsWith('L'));
        if (lEntry is null)
            return (null, $"XML-счет, начинающийся на 'L', не найден внутри архива");

        var hStream = new MemoryStream();
        var lStream = new MemoryStream();

        await using var hEntryStream = hEntry.Open();
        await using var lEntryStream = lEntry.Open();

        await hEntryStream.CopyToAsync(hStream);
        await lEntryStream.CopyToAsync(lStream);

        hStream.Position = 0;
        lStream.Position = 0;

        return (
            new XmlInvoicesExtractionResult()
            {
                 HInvoiceStream = hStream,
                 LInvoiceStream = lStream,
                 HInvoiceFilename = hEntry.FullName,
                 LInvoiceFilename = lEntry.FullName

            }, string.Empty);
    }
    private async Task<XsdValidationResult> XsdValidateInvoicesAsync(
        (MemoryStream h_invoiceMs, MemoryStream l_invoiceMs) invoices,
        (string h_schemaName, string l_schemaName) schemas)
    {
        invoices.h_invoiceMs.Position = 0;
        invoices.l_invoiceMs.Position = 0;

        var xsdValidationResult = new XsdValidationResult();

        var hSettings =
            new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaService.LoadXsdSchemaSet(schemas.h_schemaName),
                Async = true
            };

        var lSettings =
            new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaService.LoadXsdSchemaSet(schemas.l_schemaName),
                Async = true
            };


        hSettings.ValidationEventHandler +=
            (sender, e) =>
            {
                xsdValidationResult.HErrors?.Add(
                    $"[{e.Severity}] {e.Message}");
            };

        lSettings.ValidationEventHandler +=
            (sender, e) =>
            {
                xsdValidationResult.LErrors?.Add(
                    $"[{e.Severity}] {e.Message}");
            };

        var hTask = CheckWithXsdSchema(
            invoices.h_invoiceMs, hSettings, schemas.h_schemaName);

        var lTask = CheckWithXsdSchema(
            invoices.l_invoiceMs, lSettings, schemas.l_schemaName);


        await Task.WhenAll(hTask, lTask);

        return xsdValidationResult;
    }
    private async Task CheckWithXsdSchema(
        MemoryStream invoiceStream,
        XmlReaderSettings settings,
        string schemaName)
    {
        using var reader = XmlReader.Create(invoiceStream, settings);

        try
        {
            while (await reader.ReadAsync()) { }
        }
        catch (XmlException ex)
        {
            logger.LogError(
                ex,
                "Ошибка при попытке прочитать и сверить структуру XML-счета с переданной схемой {SchemaName}",
               schemaName);

            throw;
        }

        invoiceStream.Position = 0;
    }
    private async Task<InvoicePacsMatchingResult> ValidatePacsMatchingAsync(Stream h_invoiceMs, Stream l_invoiceMs)
    {
        h_invoiceMs.Position = 0;
        l_invoiceMs.Position = 0;

        var invoicePacsValidationResult = new InvoicePacsMatchingResult()
        {
            IsValid = true
        };

        var idPacsListFromH = await extractor.ExtractPacIdsAsync(h_invoiceMs, "PACIENT");
        var idPacsListFromL = await extractor.ExtractPacIdsAsync(l_invoiceMs, "PERS");

        var hashH = new HashSet<string?>(idPacsListFromH);
        var hashL = new HashSet<string?>(idPacsListFromL);

        if (!hashH.SetEquals(hashL))
        {
            var onlyInH = hashH.Except(hashL).ToList();
            var onlyInL = hashL.Except(hashH).ToList();

            invoicePacsValidationResult.Errors.Add($"PAC_ID, которые есть только в H-счете: {string.Join(",", onlyInH)}");
            invoicePacsValidationResult.Errors.Add($"PAC_ID, которые есть только в L-счете: {string.Join(",", onlyInL)}");
            invoicePacsValidationResult.IsValid = false;
        }

        return invoicePacsValidationResult;
    }
    private async Task<InvoiceWriteEligibilityResult> CheckInvoiceInDbAsync(XmlInvoicesExtractionResult invoices, JournalType journalType)
    {
        invoices.HInvoiceStream.Position = 0;
        invoices.LInvoiceStream.Position = 0;

        var h_invoiceEntity = await h_Parser.ParseToEntityAsync(invoices.HInvoiceStream);
        var l_invoiceEntity = await l_Parser.ParseToEntityAsync(invoices.LInvoiceStream);

        if (h_invoiceEntity is null)
        {
            logger.LogError(
                "После парсинга счета из XML в C# модель результат получился null");

            throw new NullReferenceException(nameof(h_invoiceEntity));
        }

        if (l_invoiceEntity is null)
        {
            logger.LogError(
                "После парсинга счета из XML в C# модель результат получился null");

            throw new NullReferenceException(nameof(l_invoiceEntity));
        }

        InvoiceCheckOutput outputParametrs = await invoiceRepository
            .CheckInvoiceExistenceAsync(
                new InvoiceCheckParameters()
                {
                    HInvoiceEntity = h_invoiceEntity,
                    LInvoiceEntity = l_invoiceEntity,
                    HInvoiceFilename = invoices.HInvoiceFilename,
                    LInvoiceFilename = invoices.LInvoiceFilename
                }, journalType);

        return new InvoiceWriteEligibilityResult()
        {
            IsValid = outputParametrs.Code == 0,
            SchetUid = outputParametrs.SchetUid,
            Error = outputParametrs.Message
        };
        
    }
    #endregion

    #region MetaPipeline
    private static string ValidateFileName(
        string zipFilename,
        string h_invoiceFilename,
        string l_invoiceFilename,
        JournalType journalType = JournalType.None)
    {

        if (journalType == JournalType.SMORX)
        {
            if (zipFilename[1..].Split('.')[0] != h_invoiceFilename[1..].Split('.')[0])
                return $"Имя файла {h_invoiceFilename} не соотвествует формату";

            if (zipFilename[1..].Split('.')[0] != l_invoiceFilename[1..].Split('.')[0])
                return $"Имя файла {l_invoiceFilename} не соотвествует формату";
        }

        if (journalType == JournalType.Inogorod)
        {
            if (zipFilename[1..].Split('.')[0] != h_invoiceFilename[1..].Split('.')[0])
                return $"Имя файла {h_invoiceFilename} не соотвествует формату";

            if (zipFilename[1..].Split('.')[0] != l_invoiceFilename[1..].Split('.')[0])
                return $"Имя файла {l_invoiceFilename} не соотвествует формату";
        }

        return string.Empty;
    }
    #endregion
}
