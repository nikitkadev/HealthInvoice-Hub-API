using System.Threading.Channels;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using HealthInvoice.Core.Dtos.Reports;
using HealthInvoice.Core.Dtos.Background;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Interfaces.Managers;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;
using HealthInvoice.Core.Interfaces.Services.Files;
using HealthInvoice.Core.Interfaces.Repository.Users;
using HealthInvoice.Core.Interfaces.Repository.Helpers;
using HealthInvoice.Core.Interfaces.Repository.Reports;
using HealthInvoice.Core.Interfaces.Repository.Invoices;
using HealthInvoice.Core.Interfaces.Repository.Rcontrol;
using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Interfaces.Services.Authorization;
using HealthInvoice.Core.Interfaces.Services.Invoices.Parsers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Mapping;
using HealthInvoice.Core.Interfaces.Services.Invoices.Exporter;
using HealthInvoice.Core.Interfaces.Services.Invoices.Management;
using HealthInvoice.Core.Interfaces.Services.Invoices.Publishers;
using HealthInvoice.Core.Interfaces.Services.Invoices.Validators;
using HealthInvoice.Core.Interfaces.Services.Invoices.Extractors;

using HealthInvoice.Application.Services;
using HealthInvoice.Application.Managers;
using HealthInvoice.Application.Services.Invoices.Parsers;
using HealthInvoice.Application.Services.Invoices.Mapping;
using HealthInvoice.Application.Services.Invoices.Management;
using HealthInvoice.Application.Services.Invoices.Extractors;

using HealthInvoice.Infrastructure.Factories;
using HealthInvoice.Infrastructure.Database.EF.Context;
using HealthInvoice.Infrastructure.Implementation.Background;
using HealthInvoice.Infrastructure.Implementation.Repository;
using HealthInvoice.Infrastructure.Implementation.Services.Files;
using HealthInvoice.Infrastructure.Implementation.Services.Authorization;
using HealthInvoice.Infrastructure.Implementation.Services.Invoices.Builders;
using HealthInvoice.Infrastructure.Implementation.Services.Invoices.Management;
using HealthInvoice.Infrastructure.Implementation.Services.Invoices.Publishers;

using HealthInvoice.Web.Shared.Config;
using HealthInvoice.Core.Dtos.Database.QueryResults;
using HealthInvoice.Core.Common;

namespace HealthInvoice.Web.Host;

public static class DependencyRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceManager, InvoiceManager>();
        services.AddScoped<IDefectsSummaryQueryRepository, DefectsSummaryQueryRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<ILkJournalRepository, LkJournalRepository>();
        services.AddScoped<IFkJournalRepository, FkJournalRepository>();
        services.AddScoped<IRControlViewSummaryData, RControlViewSummaryData>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepositoryHelper, RepositoryHelper>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IReportExporter, ReportExporter>();
        services.AddScoped<IInvoiceValidator, InvoiceValidator>();

        services.AddSingleton<IPasswordHasherService, Argon2PasswordHasherService>();
        services.AddSingleton<IFileStorageService, LocalStorageService>();
        services.AddSingleton<ISchemaService, SchemaService>();
        services.AddSingleton<ISchemaSource, FileSchemaSource>();
        services.AddSingleton<IMapper<List<LogicControlDefectDto>, ReportFormatVDto>, LkDefectReportMapper>();
        services.AddSingleton<IMapper<List<FormatControlDefectEntity>, ReportFormatFDto>, FkDefectReportMapper>();
        services.AddSingleton<IPacIdsExtractor, PacIdsExtractor>();
        services.AddSingleton<IXmlParser<ZlListEntity>, XmlParser<ZlListEntity>>();
        services.AddSingleton<IXmlParser<PersListEntity>, XmlParser<PersListEntity>>();
        services.AddSingleton<IXmlParser<ReportFormatFDto>, XmlParser<ReportFormatFDto>>();
        services.AddSingleton<IXmlParser<ReportFormatVDto>, XmlParser<ReportFormatVDto>>();
        services.AddSingleton<IQueuePublisher<InvoiceDbOperationMessage>, DbOperationQueuePublisher>();
        services.AddSingleton<IQueuePublisher<InvoiceLogicControlMessage>, DbLogicControlQueuePublisher>();
        services.AddSingleton<IFilePathService, FilePathService>();
        services.AddSingleton<IUsersDeserializer, UsersDeserializer>();
 
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, OptionsRequest request)
    {

        services.AddDbContextPool<SMODbContext>(
            options =>
            {
                var connectionString = request.IsDevelopement
                    ? request.AppSettings.ConnectionStrings.SMODB26_LOCALHOST_TEST
                    : request.AppSettings.ConnectionStrings.SMODB26_PROD;

                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(600);
                    });
               

                options.EnableDetailedErrors(request.IsDevelopement);
                options.EnableSensitiveDataLogging(request.IsDevelopement);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            });

        services.AddDbContextFactory<SMODbContext>(
            options =>
            {
                var connectionString = request.IsDevelopement
                    ? request.AppSettings.ConnectionStrings.SMODB26_LOCALHOST_TEST
                    : request.AppSettings.ConnectionStrings.SMODB26_PROD;

                options.UseSqlServer(
                    connectionString,

                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(600);
                    });


                options.EnableDetailedErrors(request.IsDevelopement);
                options.EnableSensitiveDataLogging(request.IsDevelopement);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            });


        services.AddDbContextPool<TDbContext>(
            options =>
            {

                var connectionString = request.IsDevelopement
                   ? request.AppSettings.ConnectionStrings.INOGOROD26_LOCALHOST_TEST
                   : request.AppSettings.ConnectionStrings.INOGOROD26_PROD;

                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(600);
                    });


                options.EnableDetailedErrors(request.IsDevelopement);
                options.EnableSensitiveDataLogging(request.IsDevelopement);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            });

        services.AddDbContextFactory<TDbContext>(
            options =>
            {

                var connectionString = request.IsDevelopement
                   ? request.AppSettings.ConnectionStrings.INOGOROD26_LOCALHOST_TEST
                   : request.AppSettings.ConnectionStrings.INOGOROD26_PROD;

                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(600);
                    });


                options.EnableDetailedErrors(request.IsDevelopement);
                options.EnableSensitiveDataLogging(request.IsDevelopement);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            });

        services.AddScoped<InvoiceDbContextFactory>();

        services.AddSingleton<Channel<InvoiceDbOperationMessage>>(
            sp =>
            {
                return Channel.CreateBounded<InvoiceDbOperationMessage>(1000);
            });

        services.AddSingleton<Channel<InvoiceLogicControlMessage>>(
            sp =>
            {
                return Channel.CreateBounded<InvoiceLogicControlMessage>(1000);
            });


        services.AddHostedService<InvoicesDbOperationQueueProcessor>();
        services.AddHostedService<InvoicesLogicalControlQueueProcessor>();

        return services;
    }
}