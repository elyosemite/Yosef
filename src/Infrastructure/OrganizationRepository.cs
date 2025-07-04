using AutoMapper;
using DotNetEcosystemStudy.src.Domain.Aggregates;
using DotNetEcosystemStudy.src.Infrastructure;
using DotNetEcosystemStudy.src.Infrastructure.Model;
using DotNetEcosystemStudy.src.Infrastructure.Repository;

namespace DotNetEcosystemStudy.src.Infrastructure;

public class OrganizationRepository : Repository<Organization, OrganizationDataModel, Guid, int>, IOrganizationRepository
{
    public OrganizationRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        : base(serviceScopeFactory, mapper, context => context.Organization)
    {
    }
}