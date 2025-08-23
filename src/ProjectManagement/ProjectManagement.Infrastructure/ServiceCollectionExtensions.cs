using ProjectManagement.Infrastructure.Settings;
using ProjectManagement.Infrastructure.Settings.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManagement.Infrastructure.Repository;
using ProjectManagement.Applciation.Repository;
using ProjectManagement.Domain.Events;
using Yosef.ProjectManagement.Infrastructure.DispatcherHandler;

namespace ProjectManagement.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static GlobalSettings AddGlobalSettingsServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);

        // developSelfHosted é definido dentro de um od perfis de launchSettings.json
        if (environment.IsDevelopment() && configuration.GetValue<bool>("developSelfHosted"))
        {
            // Override settings with selfHostedOverride settings
            //ConfigurationBinder.Bind(configuration.GetSection("Dev:SelfHostOverride:GlobalSettings"), globalSettings);
        }

        services.AddAutoMapper(cfg => { }, typeof(ServiceCollectionExtensions));
        services.AddSingleton(s => globalSettings);
        services.AddSingleton<IGlobalSettings, GlobalSettings>(s => globalSettings);
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return globalSettings;
    }
}