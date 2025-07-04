namespace DotNetEcosystemStudy.src.Domain.Aggregates;

public interface IEntity<TId>
{
    TId Identifier { get; }
}