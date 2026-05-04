namespace HealthInvoice.Web.Shared.Enums;

public enum Status
{
    None,
    Failed,
    Warning,
    Succes
}

public enum ErrorCode
{
    None,
    InternalError,
    ArgumentNull,
    InvalidException
}
