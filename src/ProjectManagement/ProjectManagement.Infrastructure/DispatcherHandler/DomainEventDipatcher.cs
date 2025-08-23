using Mediator;
using Microsoft.Extensions.Logging;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure.DispatcherHandler;
using Yosef.ProjectManagement.Domain.Events;

namespace Yosef.ProjectManagement.Infrastructure.DispatcherHandler;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DispatchEventAsync(IEnumerable<IHasDomainEvent> domainEvents)
    {
        _logger.LogInformation("Getting in DispatchEventAsync");
        foreach (IHasDomainEvent entity in domainEvents)
        {
            if (entity is HasDomainEventBase hasDomainEvent)
            {
                DomainEventBase[] events = hasDomainEvent.DomainEvents.ToArray();
                hasDomainEvent.ClearDomainEvents();
                _logger.LogInformation("Get domain events: {@events}", events);

                if (events.Any())
                {
                    var dispatcherNotification = new DispatcherNotification(events);
                    _logger.LogInformation("Get domain events: {@dispatcherNotification}", dispatcherNotification);
                    await _mediator.Publish(dispatcherNotification);
                }
            }
            else
            {
                _logger.LogError(
                    "Entity of type {EntityType} does not inherit from {BaseType}. Unable to clear domain events.",
                    entity.GetType().Name,
                    nameof(HasDomainEventBase));
            }
        }
    }
}