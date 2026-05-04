using HealthInvoice.Web.Shared.Enums;

namespace HealthInvoice.Web.Shared.Dtos.Responses;

public class ExceptionResponseDto
{
    public ErrorCode Code { get; set; } = ErrorCode.None;
    public Status Status { get; set; } = Status.None;
    public string? RequestId { get; set; } 
    public int HttpStatusCode { get; set; }
    public string ClientMessage { get; set; } = string.Empty;
    public string? Details { get; set; } = null;
    public DateTimeOffset ErrorDate { get; set; } = DateTimeOffset.Now;
}
