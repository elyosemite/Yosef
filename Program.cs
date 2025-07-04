using AutoMapper;
using DotNetEcosystemStudy;
using DotNetEcosystemStudy.src.Presentation.Endpoints.CreateOrganization;
using DotNetEcosystemStudy.src.Presentation.Endpoints.GetOrganization;
using DotNetEcosystemStudy.src.Infrastructure;
using DotNetEcosystemStudy.src.Infrastructure.Model;
using DotNetEcosystemStudy.Settings;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Exceptions;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog.Sinks.OpenTelemetry;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Debug()
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://localhost:4317/v1/logs";
        options.Protocol = OtlpProtocol.Grpc;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            { "service.name", "DotNetEcosystemStudy" },
            { "service.instance.id", Environment.MachineName }
        };
    })
    .CreateLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Middleware
    builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddSource("DotNetEcosystemStudy") // Nome da sua aplicação ou de instrumentação específica
            .ConfigureResource(resource => resource
                .AddService("DotNetEcosystemStudy", serviceInstanceId: Environment.MachineName))
            .AddAspNetCoreInstrumentation() // Instrumenta requisições ASP.NET Core
            .AddHttpClientInstrumentation() // Instrumenta chamadas HTTP
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317"); // Endpoint do OpenTelemetry Collector
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });
    })
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder
            .ConfigureResource(resource => resource
                .AddService("DotNetEcosystemStudy", serviceInstanceId: Environment.MachineName))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation() // Métricas de runtime do .NET
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317"); // Endpoint do OpenTelemetry Collector
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });
    });
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddOpenApiDocument(config =>
    {
        config.DocumentName = "Dotnet";
        config.Title = "Dotnet API";
        config.Version = "v1";
    });
    builder.Services.AddEFRepository();

    if (builder.Environment.IsDevelopment())
    {
        Console.WriteLine("Development environment detected.");
        builder.Configuration.AddUserSecrets<Program>();
    }

    var globalSettings = builder.Services.AddGlobalSettingsServices(builder.Configuration, builder.Environment);
    if (!globalSettings.SelfHosted)
    {
        //builder.Services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimitOptions"));
        //builder.Services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
    }

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi(config =>
        {
            config.DocumentTitle = "TodoAPI";
            config.Path = "/swagger";
            config.DocumentPath = "/swagger/{documentName}/swagger.json";
            config.DocExpansion = "list";
        });
    }

    var organization = app.MapGroup("/organization")
        .WithTags("OrganizationTag")
        .WithName("OrganizationName");

#if DEBUG
    var globalSettingsMapGroup = app.MapGroup("/globalSettings")
        .WithTags("GlobalSettingsTag")
        .WithName("GlobalSettingsName");

    globalSettingsMapGroup.MapGet("/globalSettings", () => globalSettings)
        .WithName("GetGlobalSettings")
        .Produces<GlobalSettings>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
#endif

    organization.MapPost("/organization", (OrganizationRequest req) =>
        {
            var organizationRepository = app.Services.GetRequiredService<IOrganizationRepository>();
            var mapper = app.Services.GetRequiredService<IMapper>();

            return new CreateOrganization(organizationRepository, mapper)
                .ActionAsync(req);
        })
        .WithName("CreateOrganization")
        .Produces<OrganizationDataModel>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

    organization.MapGet("/organization", ([FromQuery] Guid req) =>
        {
            var organizationRepository = app.Services.GetRequiredService<IOrganizationRepository>();

            return new GetOrganization(organizationRepository)
                .ActionAsync(new GetOrganizationRequest(req));
        })
        .WithName("GetOrganization")
        .Produces(StatusCodes.Status400BadRequest);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
    return;
}
finally
{
    Log.Information("Application ended successfully");
    Log.CloseAndFlush();
}