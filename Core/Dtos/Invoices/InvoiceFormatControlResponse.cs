namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>
/// Ответ после форматного контроля (ФК) счёта.
/// </summary>
public class InvoiceFormatControlResponse
{
    /// <summary>Успешность форматного контроля.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Будет ли счёт перезаписан (если уже существует).</summary>
    public bool WillRewrite { get; set; }

    /// <summary>Имя загруженного архива.</summary>
    public string UploadArchiveFilename { get; set; } = string.Empty;

    /// <summary>Путь к загруженному архиву на сервере.</summary>
    public string UploadArchiveFilePath { get; set; } = string.Empty;

    /// <summary>Размер архива в байтах.</summary>
    public long FileSize { get; set; }

    /// <summary>UID счёта (для перезаписи).</summary>
    public int? SchetUid { get; set; }

    /// <summary>Сообщение об ошибке (если есть).</summary>
    public string ErrorMessage { get; set; } = string.Empty;
}