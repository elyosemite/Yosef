
using System.ComponentModel.DataAnnotations.Schema;

namespace Yosef.ProjectManagement.Domain.Events;

public abstract class HasDomainEventBase : IHasDomainEvent
{
    private readonly List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}