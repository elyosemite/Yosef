using DotNetEcosystemStudy.Settings;
using DotNetEcosystemStudy.Settings.Interfaces;

namespace DotNetEcosystemStudy;

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

        services.AddSingleton(s => globalSettings);
        services.AddSingleton<IGlobalSettings, GlobalSettings>(s => globalSettings);
        return globalSettings;
    }
}