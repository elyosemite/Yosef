using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    
}