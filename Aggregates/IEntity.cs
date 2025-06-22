namespace DotNetEcosystemStudy.Aggregates;

public interface IEntity<TId>
{
    TId Identifier { get; }
}