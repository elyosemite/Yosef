using AutoMapper;
using DotNetEcosystemStudy;
using DotNetEcosystemStudy.Endpoints.CreateOrganization;
using DotNetEcosystemStudy.Endpoints.GetOrganization;
using DotNetEcosystemStudy.Infrastructure;
using DotNetEcosystemStudy.Infrastructure.Model;
using DotNetEcosystemStudy.Settings;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Middleware
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
