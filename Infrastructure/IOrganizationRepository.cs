using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Infrastructure.Repository;

namespace DotNetEcosystemStudy.Infrastructure;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    
}