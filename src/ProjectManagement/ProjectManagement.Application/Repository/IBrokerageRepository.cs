using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Applciation.Repository;

namespace ProjectManagement.Application.Repository;

public interface IBrokerageRepository : IRepository<Brokerage, Guid>
{
}
