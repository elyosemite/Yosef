namespace ProjectManagement.Infrastructure.Settings.Interfaces;

public interface IEventLoggingSettings
{
    IAzureServiceBusSettings AzureServiceBus { get; set; }
    IRabbitMqSettings RabbitMq { get; set; }
}
