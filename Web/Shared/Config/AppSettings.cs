namespace HealthInvoice.Web.Shared.Config;

public class AppSettings
{
    public string[] AllowedOrigins { get; set; } = [];
    public ApiSettings ApiSettings { get; set; } = new();
    public ConnectionStrings ConnectionStrings { get; set; } = new();
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}

public class ConnectionStrings
{
    public string SMODB18 {  get; set; } = string.Empty;
    public string INOGOROD18 { get; set; } = string.Empty;

    public string SMODB26_LOCALHOST_TEST { get; set; } = string.Empty;
    public string INOGOROD26_LOCALHOST_TEST { get; set; } = string.Empty;

    public string SMODB26_PROD { get; set; } = string.Empty;
    public string INOGOROD26_PROD { get; set; } = string.Empty;
}
