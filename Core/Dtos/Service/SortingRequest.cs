namespace HealthInvoice.Core.Dtos.Service;

public class SortingRequest
{
    public string SortBy { get; set; } = string.Empty;
    public bool IsDescending { get; set; } 
}
