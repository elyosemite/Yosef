using ProjectManagement.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ProjectManagement.Application.Repository;

namespace ProjectManagement.Infrastructure;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static void AddEFRepository(this IServiceCollection services, IGlobalSettings globalSettings)
    {
        services.AddDbContext<OrganizationContext>(options =>
        {
            Log.Information("Using connection string: {ConnectionString} in AddEFRepository", globalSettings.PostgreSql.ConnectionString);
            options.UseNpgsql(globalSettings.PostgreSql.ConnectionString);
            options.UseLoggerFactory(OrganizationContext.EfLoggerFactory);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
    }
}