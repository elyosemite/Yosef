namespace Yosef.ProjectManagement.Domain.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IHasDomainEvent> entities);
}