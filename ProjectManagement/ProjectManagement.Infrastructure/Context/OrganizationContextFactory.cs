using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.Settings;

public class OrganizationContextFactory : IDesignTimeDbContextFactory<OrganizationContext>
{
    public OrganizationContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);

        return new OrganizationContext(globalSettings);
    }
}