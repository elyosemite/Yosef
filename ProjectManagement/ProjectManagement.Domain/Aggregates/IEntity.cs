namespace ProjectManagement.Domain.Aggregates;

public interface IEntity<TId>
{
    TId Identifier { get; }
}