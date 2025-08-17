using AutoMapper;
using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Applciation.Repository;
using Microsoft.Extensions.Logging;

namespace ProjectManagement.Infrastructure.Repository;

public abstract class Repository<TAggregate, TDataModel, TAggregateId, TDataModelId> : BaseEntityFrameworkRepository, IRepository<TAggregate, TAggregateId>
    where TAggregate : class, IAggregateRoot<TAggregateId>, ITableObject<TDataModelId>
    where TDataModel : class, ITableObject<TDataModelId>
    where TAggregateId : IEquatable<TAggregateId>
    where TDataModelId : IEquatable<TDataModelId>
{
    private readonly ILogger<Repository<TAggregate, TDataModel, TAggregateId, TDataModelId>> _logger;
    public Repository(IServiceScopeFactory serviceScopeFactory, IMapper mapper, Func<OrganizationContext, DbSet<TDataModel>> getDbSet, ILogger<Repository<TAggregate, TDataModel, TAggregateId, TDataModelId>> logger)
        : base(serviceScopeFactory, mapper)
    {
        GetDbSet = getDbSet;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected Func<OrganizationContext, DbSet<TDataModel>> GetDbSet { get; private set; }

    public async Task<TAggregate> CreateAsync(TAggregate aggregate)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var dbContext = GetDatabaseContext(scope);
            var dataModel = Mapper.Map<TDataModel>(aggregate);
            await dbContext.AddAsync(dataModel);
            await dbContext.SaveChangesAsync();
            aggregate.UpdateTableRegisterId(dataModel.Id);
            return aggregate;
        }
    }

    public Task DeleteAsync(TAggregate obj)
    {
        throw new NotImplementedException();
    }

    public async Task<TAggregate?> GetByIdAsync(TAggregateId id)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var dbContext = GetDatabaseContext(scope);
            //var entity = await GetDbSet(dbContext).FindAsync(id);

            var entity = await GetDbSet(dbContext).FirstOrDefaultAsync(e =>
                    EF.Property<TAggregateId>(e, "Identifier").Equals(id));

            return Mapper.Map<TAggregate>(entity);
        }
    }

    public async Task ReplaceAsync(TAggregate obj)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var dbContext = GetDatabaseContext(scope);
            var entity = await GetDbSet(dbContext).FindAsync(obj.Id);
            if (entity != null)
            {
                var mappedEntity = Mapper.Map<TDataModel>(obj);
                dbContext.Entry(entity).CurrentValues.SetValues(mappedEntity);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task UpsertAsync(TAggregate obj)
    {
        if (obj.Id.Equals(default(TAggregateId)))
        {
            await CreateAsync(obj);
        }
        else
        {
            await ReplaceAsync(obj);
        }
    }
}