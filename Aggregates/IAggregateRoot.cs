namespace DotNetEcosystemStudy.Aggregates;

public interface IAggregateRoot<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
}
