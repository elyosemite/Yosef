using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectManagement.Infrastructure.Settings;
using Serilog;

namespace ProjectManagement.Infrastructure.Context;

public class OrganizationContextFactory : IDesignTimeDbContextFactory<OrganizationContext>
{
    public OrganizationContext CreateDbContext(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets("yosef-ProjecManagement")
            .AddCommandLine(args)
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .Build();

        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);
        Log.Information("Connection string: {@ConnectionString}", globalSettings.PostgreSql.ConnectionString);
        Log.Information("Load command line arguments: {@args}", args);
        Log.Information("GlobalSettings loaded: {@GlobalSettings}", globalSettings);

        var optionsBuilder = new DbContextOptionsBuilder<OrganizationContext>();

        if (args.Length == 0)
        {
            throw new ArgumentException("No connection string provided to the DbContextFactory.");
        }
        Log.Information("Using connection string: {ConnectionString} in OrganizationContextFactory", args[0]);
        optionsBuilder.UseNpgsql(globalSettings.PostgreSql.ConnectionString);
        optionsBuilder.UseLoggerFactory(OrganizationContext.EfLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new OrganizationContext(globalSettings, optionsBuilder.Options);
    }
}