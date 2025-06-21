using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.Infrastructure.Repository;

public interface IRepository<T, TId>
    where TId : IEquatable<TId>
    where T : class, IAggregateRoot<TId>
{
    Task<T?> GetByIdAsync(TId id);

    Task<T> CreateAsync(T obj);
    Task ReplaceAsync(T obj);
    Task UpsertAsync(T obj);
    Task DeleteAsync(T obj);
}