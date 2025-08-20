using Microsoft.EntityFrameworkCore;
using Quotation.Infrastructure.Context;
using Quotation.Infrastructure.Settings;
using Quotation.Infrastructure.Settings.Interfaces;
using Quotation.Presentation.Infrastructure;

namespace Quotation.Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuotationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);

        // Register the DbContext
        services.AddDbContext<QuotationContext>(options =>
            options.UseNpgsql(globalSettings.PostgreSql.ConnectionString));

        services.AddSingleton(s => globalSettings);
        services.AddSingleton<IGlobalSettings, GlobalSettings>(s => globalSettings);

        // Register the repository
        services.AddScoped<IQuotationRepository, QuotationRepository>();

        // Register AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(ServiceCollectionExtensions));

        return services;
    }
}