using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectManagement.Infrastructure.Settings;

namespace ProjectManagement.Infrastructure.Context;

public class OrganizationContextFactory : IDesignTimeDbContextFactory<OrganizationContext>
{
    public OrganizationContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets("yosef-ProjecManagement")
            .AddCommandLine(args)
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .Build();

        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);

        var optionsBuilder = new DbContextOptionsBuilder<OrganizationContext>();

        if (args.Length == 0)
        {
            throw new ArgumentException("No connection string provided to the DbContextFactory.");
        }

        optionsBuilder.UseNpgsql(args[0]);
        optionsBuilder.UseLoggerFactory(OrganizationContext.EfLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new OrganizationContext(globalSettings, optionsBuilder.Options);
    }
}