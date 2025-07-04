using DotNetEcosystemStudy.src.Domain.Aggregates;
using DotNetEcosystemStudy.src.Infrastructure.Repository;

namespace DotNetEcosystemStudy.src.Infrastructure;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    
}