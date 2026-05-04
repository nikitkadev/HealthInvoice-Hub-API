namespace HealthInvoice.Core.Dtos.Invoices;

/// <summary>Выходные параметры хранимой процедуры проверки счёта.</summary>
public class InvoiceCheckOutput
{
    /// <summary>Код статуса:
    /// 0 - счет может быть записан / перезаписан
    /// 1 - запись счета невозможна
    /// </summary>
    public int Code { get; set; }

    /// <summary>Если счет Uid не null, это означает, что счет есть в базе и он будет перезаписан, в случае, если Code = 0.</summary>
    public int? SchetUid { get; set; }

    /// <summary>Сформированное сообщение-ответ после выполнения процедуры.</summary>
    public string Message { get; set; } = string.Empty;
}