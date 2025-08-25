using Yosef.ProjectManagement.Domain.Events;

namespace ProjectManagement.Domain.Aggregates;

public abstract class EntityBase<TId> : HasDomainEventBase
{
    public TId Identifier { get; protected set; }
}