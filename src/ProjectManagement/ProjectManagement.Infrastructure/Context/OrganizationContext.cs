using ProjectManagement.Infrastructure.Configuration;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Infrastructure.Context;

public class OrganizationContext : DbContext
{
    protected readonly IGlobalSettings _globalSettings;

    public OrganizationContext(IGlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_globalSettings.PostgreSql.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OrganizationDataModel> Organization { get; set; }
}