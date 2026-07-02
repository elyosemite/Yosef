using AutoMapper;
using ProjectManagement.Presentation.Endpoints.CreateBrokerage;
using ProjectManagement.Presentation.Endpoints.GetBrokerage;
using ProjectManagement.Presentation.Endpoints.RenameBrokerage;
using ProjectManagement.Presentation.Endpoints.CreateProject;
using ProjectManagement.Presentation.ExceptionHandling;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Settings;
using Serilog;
using Serilog.Exceptions;
using FluentValidation;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Events;
using System.Diagnostics;
using ProjectManagement.Infrastructure.Metric;
using ProjectManagement.Application;

namespace ProjectManagement.Presentation;

public class Program
{
    private static readonly Random _random = new Random();
    internal static readonly ActivitySource GreeterActivitySource = new ActivitySource("ApplicationBeginning");

    internal static async Task Delay<T>(ILogger<T> logger)
    {
        using var activity = GreeterActivitySource.StartActivity("Simulate delay");
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
        else if (randomlyChoosedNumber % 13 == 0)
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
                options.Endpoint = "http://otel-collector:4317/v1/logs";
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

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddJsonFile("secrets.json", optional: true)
                .AddEnvironmentVariables();

            Log.Information("Arguments: {@args}", args);

            var globalSettings = builder.Services.AddGlobalSettingsServices(builder.Configuration, builder.Environment);

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("Yosef")
                    .AddSource(GreeterActivitySource.Name)
                    .ConfigureResource(resource => resource
                        .AddService("Yosef", serviceInstanceId: Environment.MachineName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(p =>
                    {
                        p.SetDbStatementForText = true;
                    })
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://otel-collector:4317");
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
            })
            .WithMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .ConfigureResource(resource => resource
                        .AddService("Yosef", serviceInstanceId: Environment.MachineName))
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(Metrics.GreeterMeter.Name)
                    .AddMeter(Metrics.CreateBrokerageMeter.Name)
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddPrometheusExporter(options =>
                    {
                        options.ScrapeResponseCacheDurationMilliseconds = 10000;
                        options.ScrapeEndpointPath = "/metrics";
                    })
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://otel-collector:4317");
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "Dotnet";
                config.Title = "Dotnet API";
                config.Version = "v1";
            });
            builder.Services.AddEFRepository(globalSettings);
            builder.Services.AddApplication();

            if (builder.Environment.IsDevelopment())
            {
                Console.WriteLine("Development environment detected.");
                builder.Configuration.AddUserSecrets<Program>();
            }

            Log.Information("Global settings loaded: {@GlobalSettings}", globalSettings);
            if (!globalSettings.SelfHosted)
            {
                //builder.Services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimitOptions"));
                //builder.Services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            }

            var app = builder.Build();

            app.UseExceptionHandler();
            app.UseStatusCodePages();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectManagement API V.1");
                    c.RoutePrefix = "swagger";
                });
            }

            var brokerages = app.MapGroup("/api/v1/brokerages")
                .WithTags("Brokerages");

#if DEBUG
            var globalSettingsMapGroup = app.MapGroup("/globalSettings")
                .WithTags("GlobalSettings");

            globalSettingsMapGroup.MapGet("/", () => globalSettings)
                .WithName("GetGlobalSettings")
                .Produces<GlobalSettings>(StatusCodes.Status200OK);
#endif

            brokerages.MapCreateBrokerage(app);
            brokerages.MapGetBrokerage(app);
            brokerages.MapRenameBrokerage();
            brokerages.MapCreateProject(app);

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
