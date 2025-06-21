using DotNetEcosystemStudy;
using DotNetEcosystemStudy.DataModel;
using DotNetEcosystemStudy.Endpoints;
using DotNetEcosystemStudy.Settings;

var builder = WebApplication.CreateBuilder(args);

// Middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Dotnet";
    config.Title = "Dotnet API";
    config.Version = "v1";
});

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

organization.MapGet("/organization", () => GetOrganization.Action())
    .WithName("GetOrganization")
    .Produces<OrganizationDataModel>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

organization.MapPost("/organization", (OrganizationDataModel organizationDataModel)
    => new CreateOrganization(globalSettings).Action(organizationDataModel.Name, organizationDataModel.ContributorsCount))
        .WithName("CreateOrganization")
        .Produces<OrganizationDataModel>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

app.Run();
