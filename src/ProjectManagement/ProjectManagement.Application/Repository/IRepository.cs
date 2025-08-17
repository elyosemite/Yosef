using ProjectManagement.Domain.Aggregates;

namespace ProjectManagement.Applciation.Repository;

public interface IRepository<TAggregate, TAggregateId>
    where TAggregateId : IEquatable<TAggregateId>
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
    Task<TAggregate?> GetByIdAsync(TAggregateId id);

    Task<TAggregate> CreateAsync(TAggregate obj);
    Task ReplaceAsync(TAggregate obj);
    Task UpsertAsync(TAggregate obj);
    Task DeleteAsync(TAggregate obj);
}