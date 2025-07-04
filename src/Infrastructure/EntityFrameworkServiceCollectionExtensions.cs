using DotNetEcosystemStudy.src.Infrastructure;
using Infrastructure.src.Infrastructure.Context;

namespace DotNetEcosystemStudy.src.Infrastructure;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static void AddEFRepository(this IServiceCollection services)
    {
        services.AddDbContext<OrganizationContext>();
        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
    }
}