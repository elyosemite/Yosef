namespace Yosef.ProjectManagement.Domain.Events;

public interface IHasDomainEvent
{
    IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
}