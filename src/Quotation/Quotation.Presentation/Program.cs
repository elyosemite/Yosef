using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quotation.Infrastructure.Metric;
using Quotation.Presentation;
using Quotation.Presentation.Domain;
using Quotation.Presentation.Infrastructure;
using Quotation.Presentation.Infrastructure.Model;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.OpenTelemetry;


public class Program
{
    public static readonly ActivitySource _activitySource = new ActivitySource("ApplicationBeginning");

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithSpan()
            .Enrich.WithProperty("TraceFlags", () => Activity.Current?.ActivityTraceFlags.ToString())
            .Enrich.WithProperty("ParentSpanId", () => Activity.Current?.ParentSpanId.ToString())
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                                     "traceId={TraceId} spanId={SpanId}{NewLine}{Exception}")
            .WriteTo.Debug()
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = "http://otel-collector:4317/v1/logs";
                options.Protocol = OtlpProtocol.Grpc;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    { "service.name", "quotation" },
                    { "service.instance.id", Environment.MachineName }
                };
            })
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddQuotationInfrastructure(builder.Configuration);
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Quotation API",
                Version = "v1",
                Description = "Quotation API by using .NET 9 with Swagger (OpenAPI)",
                Contact = new OpenApiContact
                {
                    Name = "Admin",
                    Email = "yurifullstack@gmail.com"
                }
            });
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("Quotation")
                    .AddSource(_activitySource.Name)
                    .ConfigureResource(resource => resource
                        .AddService("Quotation", serviceInstanceId: Environment.MachineName))
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
                //.AddConsoleExporter();
            })
            .WithMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .ConfigureResource(resource => resource
                        .AddService("Quotation", serviceInstanceId: Environment.MachineName))
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(Metrics.QuotationMeter.Name)
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
                //.AddConsoleExporter();
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quotation API v1");
                c.RoutePrefix = "/api/quotation";
            });
        }

        app.UseHttpsRedirection();

        app.MapPost("/create", async ([FromBody] QuotationDataModel request, IQuotationRepository repository, IMapper mapper) =>
        {
            var domain = mapper.Map<QuotationDomain>(request);
            var result = await repository.CreateAsync(domain);

            Metrics.CountQuotation.Add(1);

            return Results.Ok(result);
        })
        .WithName("quotation");

        app.Run();

    }
}