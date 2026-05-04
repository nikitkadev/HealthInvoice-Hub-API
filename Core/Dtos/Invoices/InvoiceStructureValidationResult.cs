namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>Класс-ответ полной проверки входящего архива с счетами.</summary>
public class InvoiceStructureValidationResult
{
    /// <summary>Размер отрпавленного архива.</summary>
    public long ArchiveFileSize { get; set; }

    /// <summary>Имя отправленного архива.</summary>
    public string ArchiveFilename { get; set; } = string.Empty;

    /// <summary>Путь к архиву на сервере.</summary>
    public string ArchiveFilePath { get; set; } = string.Empty;


    /// <summary>Вычисляемый статус проверки отправленного архива.</summary>
    public bool IsSuccess => IntegrityResult.IsValid && ComplianceResult.IsValid;

    /// <summary>Флаг, обозначающий будет ли счет перезаписан в базе или нет.</summary>
    public bool WillRewrite => ComplianceResult.InvoiceWriteEligibilityResult.SchetUid.HasValue;

    /// <summary>Сформированное сообщение об ошибке.</summary>
    public string ClientErrorMessage =>
        !string.IsNullOrEmpty(IntegrityResult.ClientErrorMessage)
        ? IntegrityResult.ClientErrorMessage
            : !string.IsNullOrEmpty(ComplianceResult.ClientErrorMessage)
            ? ComplianceResult.ClientErrorMessage : string.Empty;


    /// <summary>Результат проверки целостности архива и файлов внутри.</summary>
    public required IntegrityValidationResult IntegrityResult { get; set; } = new();

    /// <summary>Проверка внутренних счетов на соответствие схеме и некоторым бизнес требованиям.</summary>
    public required ComplianceValidationResult ComplianceResult { get; set; } = new(); 
}

/// <summary>Результат проверки целостности архива и файлов внутри.</summary>
public class IntegrityValidationResult
{
    /// <summary>Флаг валидности проверки на целостность.</summary>
    public bool IsValid { get; set; }

    /// <summary>Сформированное сообщение об ошибке для клиента.</summary>
    public string ClientErrorMessage { get; set; } = string.Empty;
}

/// <summary>Проверка внутренних счетов на соответствие схеме и некоторым бизнес требованиям.</summary>
public class ComplianceValidationResult
{
    /// <summary>Флаг валидности проверки на соответствие.</summary>
    public bool IsValid =>
        XsdValidationResult.IsValid &&
        InvoiceWriteEligibilityResult.IsValid &&
        PacIdsValidationResult.IsValid;

    /// <summary>Сформированное сообщение об ошибке для клиента.</summary>
    public string ClientErrorMessage { get; set; } = string.Empty;

    /// <summary>Результат проверки на соответствие XSD-схемы.</summary>
    public XsdValidationResult XsdValidationResult { get; set; } = new();

    /// <summary>Результат проверки счета на наличие такого же счета внутри базы данных.</summary>
    public InvoiceWriteEligibilityResult InvoiceWriteEligibilityResult { get; set; } = new();

    /// <summary>Результат соответствие Pac Ids внутри счетов H и L.</summary>
    public InvoicePacsMatchingResult PacIdsValidationResult { get; set; } = new();
}

/// <summary>Результат проверки на соответствие XSD-схемы.</summary>
public class XsdValidationResult
{
    /// <summary>Список несоответствий L-счета схеме.</summary>
    public List<string> HErrors { get; set; } = [];

    /// <summary>Список несоответствий H-счета схеме.</summary>
    public List<string> LErrors { get; set; } = [];

    /// <summary>Флаг валидности проверки на соответствие XSD-схеме.</summary>
    public bool IsValid => HErrors.Count == 0 && LErrors.Count == 0;
}

/// <summary>Результат проверки счета на наличие такого же счета внутри базы данных.</summary>
public class InvoiceWriteEligibilityResult
{
    /// <summary>Флаг, обозначающий будет ли перезаписан счет.</summary>
    public bool IsValid { get; set; }

    /// <summary>Uid перезаписанного счета. null, если нечего перезаписывать.</summary>
    public int? SchetUid { get; set; }

    /// <summary>Сформированное сообщение об ошибке.</summary>
    public string Error { get; set; } = string.Empty;
}

/// <summary>Результат соответствие Pac Ids внутри счетов H и L.</summary>
public class InvoicePacsMatchingResult
{
    /// <summary>Флаг валидности проверки на соответствие Pac Ids внутри счетов H и L.</summary>
    public bool IsValid { get; set; }

    /// <summary>Сформированный список ошибок при попытке сравнить Pac Ids.</summary>
    public List<string> Errors { get; set; } = [];
}