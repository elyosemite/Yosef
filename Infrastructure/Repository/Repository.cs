using AutoMapper;
using DotNetEcosystemStudy.Aggregates;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DotNetEcosystemStudy.Infrastructure.Repository;

public abstract class Repository<TAggregate, TDataModel, TId> : BaseEntityFrameworkRepository, IRepository<TAggregate, TId>
    where TAggregate : class, IAggregateRoot<TId>
    where TDataModel : class, ITableObject<TId>
    where TId : class, IEquatable<TId>
{
    public Repository(IServiceScopeFactory serviceScopeFactory, IMapper mapper, Func<OrganizationContext, DbSet<TAggregate>> getDbSet)
        : base(serviceScopeFactory, mapper)
    {
        GetDbSet = getDbSet;
    }

    protected Func<OrganizationContext, DbSet<TAggregate>> GetDbSet { get; private set; }

    public async Task<TAggregate> CreateAsync(TAggregate obj)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var dbContext = GetDatabaseContext(scope);
            //obj.SetNewId();
            var entity = Mapper.Map<TDataModel>(obj);

            // Create Data Model

            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            //obj.Id = entity.Id;
            return obj;
        }
    }

    public Task DeleteAsync(TAggregate obj)
    {
        throw new NotImplementedException();
    }

    public Task<TAggregate?> GetByIdAsync(TId id)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceAsync(TAggregate obj)
    {
        throw new NotImplementedException();
    }

    public Task UpsertAsync(TAggregate obj)
    {
        throw new NotImplementedException();
    }
}