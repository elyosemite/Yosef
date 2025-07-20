using ProjectManagement.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static void AddEFRepository(this IServiceCollection services)
    {
        services.AddDbContext<OrganizationContext>();
        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
    }
}