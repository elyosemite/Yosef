namespace DotNetEcosystemStudy.Aggregates;

public interface IEntity<TId>
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    TId Identifier { get; }
}