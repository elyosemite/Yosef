using AutoMapper;
using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Application.Repository;
using Microsoft.Extensions.Logging;

namespace ProjectManagement.Infrastructure;

public class OrganizationRepository : Repository<Organization, OrganizationDataModel, Guid, int>, IOrganizationRepository
{
    public OrganizationRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<OrganizationRepository> logger)
        : base(serviceScopeFactory, mapper, context => context.Organization, logger)
    {
    }
}