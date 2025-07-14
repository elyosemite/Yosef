namespace ProjectManagement.Domain.Aggregates;

public interface IAggregateRoot<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
}
