using System.Text;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

using Serilog;

using HealthInvoice.Web.Shared.Enums;
using HealthInvoice.Web.Shared.Config;
using HealthInvoice.Web.Shared.Dtos.Responses;

namespace HealthInvoice.Web.Host;

public class App
{
    public static async Task Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder();
        var appSettings = new AppSettings();

        builder.Configuration.Bind(appSettings);

        builder.Host.UseWindowsService();
        builder.Services.AddOpenApi();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(
            options =>
            {
                options.Cookie.Name = "HealthInvoiceHub.Session";
                options.IdleTimeout = TimeSpan.FromHours(9);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.Path = "/";
            });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.AddCoreServices();
        builder.Services.AddInfrastructureServices(
            new()
            {
                AppSettings = appSettings,
                IsDevelopement = builder.Environment.IsDevelopment(),
                ArchivePath = Path.Combine(builder.Environment.ContentRootPath, "archive")
            });
        builder.Services.Configure<FormOptions>(
            options =>
            {
                options.MultipartHeadersLengthLimit = 64 * 1024;
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
                options.ValueLengthLimit = 100 * 1024 * 1024;
            });
        builder.Services.AddCors(
            options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(appSettings.AllowedOrigins)
                            .WithExposedHeaders("Content-Disposition")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        builder.Host.UseSerilog();
        builder.WebHost.ConfigureKestrel(
            options =>
            {
                options.ListenAnyIP(8090);
                options.AddServerHeader = false;
            });

        var app = builder.Build();

        app.UseExceptionHandler(
            errorApp =>
            {
                errorApp.Run(
                    async context =>
                    {
                        context.Response.ContentType = "application/json";

                        var ex = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                        var requestId = context.TraceIdentifier;
                        var logger = context.RequestServices.GetRequiredService<ILogger<App>>();

                        logger.LogError(
                            ex,
                            "Необработанное исключение (RequestId: {RequestId})",
                            requestId);

                        var response = new ExceptionResponseDto()
                        {
                            Code = ErrorCode.InternalError,
                            Status = Status.Failed,
                            HttpStatusCode = 500,
                            ClientMessage = "Внутренняя ошибка сервера",
                            Details = app.Environment.IsDevelopment() ? ex?.ToString() : null,
                            RequestId = requestId,
                            ErrorDate = DateTimeOffset.Now,
                        };

                        await context.Response.WriteAsJsonAsync(response);

                    });
            });
        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.UseSession();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}