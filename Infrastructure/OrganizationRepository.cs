using AutoMapper;
using DotNetEcosystemStudy.Infrastructure.Model;
using DotNetEcosystemStudy.Infrastructure.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DotNetEcosystemStudy.Infrastructure;

public class OrganizationRepository : Repository<Aggregates.Organization, Organization, Guid, int>, IOrganizationRepository
{
    public OrganizationRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        : base(serviceScopeFactory, mapper, context => context.Organization)
    {
    }
}