using Yosef.ProjectManagement.Domain.Events;

namespace Yosef.ProjectManagement.Infrastructure.EventProcessor;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IHasDomainEvent> entities);
}