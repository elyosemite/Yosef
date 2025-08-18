using Mediator;
using EventProcessor.Settings.Interfaces;
using EventProcessor.Settings;
using EventProcessor.RabbitMQ;
using EventProcessor.Repository;

namespace EventProcessor.Worker;

public static class ServiceCollectionExtensions
{
    public static GlobalSettings AddWorker(this IServiceCollection services,
        IConfiguration configuration)
    {
        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);
        
        services.AddSingleton(s => globalSettings);
        services.AddSingleton<IGlobalSettings, GlobalSettings>(s => globalSettings);
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddSingleton<IOutboxRepository, OutboxRepository>();

        services.AddMediator(
            (MediatorOptions options) =>
            {
                options.Assemblies = [typeof(ServiceCollectionExtensions)];
                options.ServiceLifetime = ServiceLifetime.Singleton;
            }
        );

        return globalSettings;
    }
}