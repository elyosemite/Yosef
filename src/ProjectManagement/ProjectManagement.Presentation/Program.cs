using AutoMapper;
using ProjectManagement.Presentation.Endpoints.CreateOrganization;
using ProjectManagement.Presentation.Endpoints.GetOrganization;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Settings;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Exceptions;
using FluentValidation;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog.Sinks.OpenTelemetry;
using Observability;
using Serilog.Events;
using System.Diagnostics;

namespace ProjectManagement.Presentation;

public class Program
{
    private static readonly Random _random = new Random();
    private static readonly ActivitySource _greeterActivitySource = new ActivitySource("ApplicationBeginning");

    private static async Task Delay<T>(ILogger<T> logger)
    {
        using var activity = _greeterActivitySource.StartActivity("Simulate delay");
        activity?.SetStatus(ActivityStatusCode.Unset, "The status for this operation has not been set yet");
        activity?.AddEvent(new ActivityEvent("Delay started"));
        activity?.AddEvent(new ActivityEvent("Delay finished (if any)"));
        activity?.AddEvent(new ActivityEvent("Delay method completed"));

        activity?.SetTag("delay.timestamp", DateTime.UtcNow.ToString("o"));
        activity?.SetTag("delay.threadId", Environment.CurrentManagedThreadId);
        activity?.SetTag("delay.environment", Environment.MachineName);

        var randomlyChoosedNumber = _random.Next(1, 10000);
        activity?.AddEvent(new ActivityEvent($"Random number generated {randomlyChoosedNumber}"));
        if (randomlyChoosedNumber % 7 == 0)
        {
            var waitingTime = _random.Next(1, 15);
            logger.LogInformation($"Number {randomlyChoosedNumber} devided by 7, waiting {waitingTime} seconds...");

            activity?.SetTag("delay.randomNumber", randomlyChoosedNumber);
            activity?.SetTag("delay.isMultipleOf7", randomlyChoosedNumber % 7 == 0);

            await Task.Delay(TimeSpan.FromSeconds(waitingTime));
        }
        else if (randomlyChoosedNumber % 13 == 0) // It means an error
        {
            var waitingTime = _random.Next(1, 15);
            logger.LogInformation($"Waiting {waitingTime} seconds...");
            activity?.SetStatus(ActivityStatusCode.Error, "An error occurred during the operation");

            activity?.SetTag("delay.isMultipleOf13", randomlyChoosedNumber);
            logger.LogError($"Number {randomlyChoosedNumber} devided by 13 means an error.");
        }
        else
        {
            var waitingTime = _random.Next(5, 30);
            logger.LogInformation($"Waiting {waitingTime} seconds...");
            activity?.SetTag("delay.NotMultipleOf7Or13", randomlyChoosedNumber);
            activity?.SetStatus(ActivityStatusCode.Ok, "An occurred default operation during the operation");
        }
    }

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Minecraft.EntityFrameworkCore", LogEventLevel.Information)
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
                { "service.name", "Yosef" },
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
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("Yosef")
                    .AddSource(_greeterActivitySource.Name)
                    .ConfigureResource(resource => resource
                        .AddService("Yosef", serviceInstanceId: Environment.MachineName))
                    .AddAspNetCoreInstrumentation() // Instrumenta requisições ASP.NET Core
                    .AddHttpClientInstrumentation() // Instrumenta chamadas HTTP
                    .AddEntityFrameworkCoreInstrumentation(p =>
                    {
                        p.SetDbStatementForText = true;
                    })
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317"); // Endpoint do OpenTelemetry Collector
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    })
                    .AddConsoleExporter();
            })
            .WithMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .ConfigureResource(resource => resource
                        .AddService("Yosef", serviceInstanceId: Environment.MachineName))
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(Metrics.GreeterMeter.Name)
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddPrometheusExporter(options =>
                    {
                        options.ScrapeResponseCacheDurationMilliseconds = 10000; // Cache de 10 segundos
                        options.ScrapeEndpointPath = "/metrics"; // Caminho do endpoint de métricas
                    })
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317"); // Endpoint do OpenTelemetry Collector
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
                    //.AddConsoleExporter();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
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
            organization.MapPost("/organization", async ([FromBody] OrganizationRequest req, IValidator<OrganizationRequest> validator, ILogger<CreateOrganization> logger) =>
                {
                    using var activity = _greeterActivitySource.StartActivity("GreeterActivity");
                    logger.LogInformation("Sending greeting and creating organization");

                    Metrics.CountGreetings.Add(1); // It goes to Prometheus

                    var organizationRepository = app.Services.GetRequiredService<IOrganizationRepository>();
                    var mapper = app.Services.GetRequiredService<IMapper>();

                    await Delay(logger);

                    var org = await new CreateOrganization(organizationRepository, mapper, logger, validator).ActionAsync(req);
                    activity?.AddEvent(new ActivityEvent("The application just created one org"));

                    Metrics.CountOrganizationsCreated.Add(1);  // It goes to Prometheus
                    activity?.SetTag("greeting", "Hello World!");
                    activity?.SetTag("organization", req.OrganizationName);

                    return org;
                })
                .WithName("CreateOrganization")
                .Produces<OrganizationDataModel>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            organization.MapGet("/organization", async ([FromQuery] Guid req, ILogger<GetOrganization> logger) =>
                {
                    using var activity = _greeterActivitySource.StartActivity("GetOrganization");
                    logger.LogInformation($"Get organization by id = {req}");

                    var organizationRepository = app.Services.GetRequiredService<IOrganizationRepository>();

                    await Delay(logger);

                    activity?.SetTag("organizationId", req);

                    var org = await new GetOrganization(organizationRepository)
                        .ActionAsync(new GetOrganizationRequest(req));

                    return org;
                })
                .WithName("GetOrganization")
                .Produces(StatusCodes.Status400BadRequest);

            app.MapPrometheusScrapingEndpoint();
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
    }
}