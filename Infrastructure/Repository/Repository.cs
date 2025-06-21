using AutoMapper;
using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DotNetEcosystemStudy.Infrastructure.Repository;

public abstract class Repository<TAggregate, TDataModel, TId, TDataModelId> : BaseEntityFrameworkRepository, IRepository<TAggregate, TId>
    where TAggregate : class, IAggregateRoot<TId>
    where TDataModel : class, ITableObject<TDataModelId>
    where TId : IEquatable<TId>
    where TDataModelId : IEquatable<TDataModelId>
{
    public Repository(IServiceScopeFactory serviceScopeFactory, IMapper mapper, Func<OrganizationContext, DbSet<TDataModel>> getDbSet)
        : base(serviceScopeFactory, mapper)
    {
        GetDbSet = getDbSet;
    }

    protected Func<OrganizationContext, DbSet<TDataModel>> GetDbSet { get; private set; }

    public async Task<TAggregate> CreateAsync(TAggregate aggregate)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var dbContext = GetDatabaseContext(scope);
            //aggregate.SetNewId();

            var dataModel = Mapper.Map<TDataModel>(aggregate);

            if (dataModel is Model.Organization organization)
            {
                Console.WriteLine($"[Before CreateAsync] - Organization Id: {organization.Id}");
                Console.WriteLine($"[Before CreateAsync] - Organization Identifier: {organization.Identifier}");
                Console.WriteLine($"[Before CreateAsync] - Organization Name: {organization.OrganizationName}");
                Console.WriteLine($"[Before CreateAsync] - Organization ContributorsCount: {organization.ContributorsCount}");
                Console.WriteLine($"[Before CreateAsync] - Organization Secret: {organization.Secret}");
                
                foreach (var project in organization.Projects)
                {
                    Console.WriteLine($"\n[Before CreateAsync] - Project Id: {project.Id}");
                    Console.WriteLine($"[Before CreateAsync] - Project Identifier: {project.Identifier}");
                    Console.WriteLine($"[Before CreateAsync] - Project Name: {project.Name}");
                    Console.WriteLine($"[Before CreateAsync] - Project Description: {project.Description}");
                }
            }

            // Create Data Model

            await dbContext.AddAsync(dataModel);
            await dbContext.SaveChangesAsync();

            if (aggregate is Model.Organization organization2)
            {
                Console.WriteLine($"[Before CreateAsync] - Organization Id: {organization2.Id}");
                Console.WriteLine($"[After CreateAsync] - Organization Identifier: {organization2.Identifier}");
                Console.WriteLine($"[After CreateAsync] - Organization Name: {organization2.OrganizationName}");
                Console.WriteLine($"[After CreateAsync] - Organization ContributorsCount: {organization2.ContributorsCount}");
                Console.WriteLine($"[After CreateAsync] - Organization Secret: {organization2.Secret}");
                
                foreach (var project in organization2.Projects)
                {
                    Console.WriteLine($"\n[After CreateAsync] - Project Identifier: {project.Identifier}");
                    Console.WriteLine($"[After CreateAsync] - Project Name: {project.Name}");
                    Console.WriteLine($"[After CreateAsync] - Project Description: {project.Description}");
                }
            }

            //aggregate.Id = dataModel.Id;
            return aggregate;
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