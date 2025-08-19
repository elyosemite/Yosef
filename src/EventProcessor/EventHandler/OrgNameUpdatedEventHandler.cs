using EventProcessor.Events;
using EventProcessor.RabbitMQ;
using Mediator;

namespace EventProcessor.EventHandler;

public class OrgNameUpdatedEventHandler : INotificationHandler<OrgNameUpdatedEvent>
{
    private readonly ILogger<OrgNameUpdatedEventHandler> _logger;
    private readonly IRabbitMqPublisher _publisher;

    public OrgNameUpdatedEventHandler(ILogger<OrgNameUpdatedEventHandler> logger, IRabbitMqPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask Handle(OrgNameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OrgNameUpdatedEvent for Organization: {@notification}", notification);
        await _publisher.PublishAsync(notification); // Publish the event to RabbitMQ queue in order to processed it later by another handler.
    }
}