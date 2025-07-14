using AutoMapper;
using ProjectManagement.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure.Repository;

public abstract class BaseEntityFrameworkRepository
{
    protected BaseEntityFrameworkRepository(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Mapper = mapper;
    }

    protected IServiceScopeFactory ServiceScopeFactory { get; private set; }
    protected IMapper Mapper { get; private set; }

    public OrganizationContext GetDatabaseContext(IServiceScope serviceScope)
    {
        return serviceScope.ServiceProvider.GetRequiredService<OrganizationContext>();
    }
}