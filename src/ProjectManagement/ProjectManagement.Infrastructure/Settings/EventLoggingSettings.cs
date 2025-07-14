using ProjectManagement.Infrastructure.Settings.Interfaces;

namespace ProjectManagement.Infrastructure.Settings;

public class EventLoggingSettings : IEventLoggingSettings
{
    public IAzureServiceBusSettings AzureServiceBus { get; set; }
    public IRabbitMqSettings RabbitMq { get; set; }

    public EventLoggingSettings()
    {
        AzureServiceBus = new AzureServiceBusSettings();
        RabbitMq = new RabbitMqSettings();
    }
}