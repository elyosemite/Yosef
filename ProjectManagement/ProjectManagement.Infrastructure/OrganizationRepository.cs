using AutoMapper;
using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure;

public class OrganizationRepository : Repository<Organization, OrganizationDataModel, Guid, int>, IOrganizationRepository
{
    public OrganizationRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        : base(serviceScopeFactory, mapper, context => context.Organization)
    {
    }
}