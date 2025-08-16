using Yosef.ProjectManagement.Domain.Events;

namespace Yosef.ProjectManagement.Infrastructure.EventProcessor;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    public async Task DispatchEventsAsync(IEnumerable<IHasDomainEvent> entitiesWithEvents)
    {
        foreach (IHasDomainEvent entity in entitiesWithEvents)
        {
            if (entity is HasDomainEventBase hasDomainEvent)
            {
                DomainEventBase[] events = hasDomainEvent.DomainEvents.ToArray();
                hasDomainEvent.ClearDomainEvents();

                foreach (DomainEventBase domainEvent in events)
                {
                    // Here you would typically publish the domain event to a message bus or event handler
                    // For example:
                    // await _eventBus.PublishAsync(domainEvent);

                    // Simulating async operation
                    await Task.Delay(10);
                }
            }
        }
    }
}