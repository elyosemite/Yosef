using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Applciation.Repository;

namespace ProjectManagement.Application.Repository;

public interface IOrganizationRepository : IRepository<Organization, Guid>
{
    
}