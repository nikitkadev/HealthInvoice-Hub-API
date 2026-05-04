namespace HealthInvoice.Core.Common;

/// <summary>Перечисляемый тип, обозначающий тип проводимой операции в базе данных.</summary>
public enum DbOperation
{
    None = 0,
    Insert = 1,
    Update = 2
}

/// <summary>Перечисляемый тип, обозначающий тип журнала, в котором работает пользователь.</summary>
public enum JournalType
{
    None = 0,
    SMORX = 1,
    Inogorod = 2
}

/// <summary>Перечисляемый тип, обозначающий статус медико-экономического контроля.</summary>
public enum MECStatus
{
    None = 0,
    Pending = 1,      
    Processing = 2,   
    Success = 3,      
    Error = 4,        
    Failed = 5        
}

/// <summary>Перечисляемый тип, обозначающий формат счета, по которому проводится операция.</summary>
public enum InvoiceFormatType
{
    H = 1,
    L = 2
}
