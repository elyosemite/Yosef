using AutoMapper;
using ProjectManagement.Domain.Aggregates;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Application.Repository;
using Microsoft.Extensions.Logging;

namespace ProjectManagement.Infrastructure;

public class BrokerageRepository : Repository<Brokerage, BrokerageDataModel, Guid, int>, IBrokerageRepository
{
    public BrokerageRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<BrokerageRepository> logger)
        : base(serviceScopeFactory, mapper, context => context.Brokerage, logger)
    {
    }
}
