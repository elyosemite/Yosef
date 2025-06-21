using Infrastructure.Context;

namespace DotNetEcosystemStudy.Infrastructure;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static void AddEFRepository(this IServiceCollection services)
    {
        services.AddDbContext<OrganizationContext>();
        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
    }
}