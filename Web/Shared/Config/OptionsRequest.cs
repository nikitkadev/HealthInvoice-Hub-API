namespace HealthInvoice.Web.Shared.Config;

public class OptionsRequest
{
    public bool IsDevelopement { get; set; }
    public string ArchivePath { get; set; } = string.Empty;
    public AppSettings AppSettings { get; set; } = new();
}
