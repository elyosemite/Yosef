namespace DotNetEcosystemStudy.Infrastructure.Repository;

public abstract class Repository<TEntity, TDataModel, TId>
    where TEntity : class, IAggregateRoot<TId>
    where TDataModel : class, ITableObject
    where TId : class, IEquatable<TId>
{
    
}