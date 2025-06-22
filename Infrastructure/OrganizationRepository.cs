using AutoMapper;
using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Infrastructure.Model;
using DotNetEcosystemStudy.Infrastructure.Repository;

namespace DotNetEcosystemStudy.Infrastructure;

public class OrganizationRepository : Repository<Organization, OrganizationDataModel, Guid, int>, IOrganizationRepository
{
    public OrganizationRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        : base(serviceScopeFactory, mapper, context => context.Organization)
    {
    }
}