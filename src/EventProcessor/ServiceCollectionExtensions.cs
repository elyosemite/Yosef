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
        try
        {
            ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error to bind GlobalSettings", ex);
        }
        
        services.AddSingleton(s => globalSettings);
        services.AddSingleton<IGlobalSettings, GlobalSettings>(s => globalSettings);
        services.AddSingleton<IRabbitMqPublisher>(rabbitmq =>
        {
            return RabbitMqPublisher.CreateAsync(globalSettings).GetAwaiter().GetResult();
        });
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