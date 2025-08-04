using ProjectManagement.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static void AddEFRepository(this IServiceCollection services)
    {
        services.AddDbContext<OrganizationContext>(options =>
        {
            options.UseLoggerFactory(OrganizationContext.EfLoggerFactory);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
    }
}