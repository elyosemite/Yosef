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
            .Build();

        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);

        return new OrganizationContext(globalSettings);
    }
}