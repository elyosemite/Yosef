using EventProcessor.RabbitMQ;
using Mediator;

namespace EventProcessor.EventHandler;

public record GenericEvent(string Payload) : INotification;

public class OrgNameUpdatedEventHandler : INotificationHandler<GenericEvent>
{
    private readonly ILogger<OrgNameUpdatedEventHandler> _logger;
    private readonly IRabbitMqPublisher _publisher;

    public OrgNameUpdatedEventHandler(ILogger<OrgNameUpdatedEventHandler> logger, IRabbitMqPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask Handle(GenericEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OrgNameUpdatedEvent for Organization: {@notification}", notification);
        await _publisher.PublishPayloadAsync(notification.Payload);
    }
}