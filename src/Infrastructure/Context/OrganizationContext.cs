using DotNetEcosystemStudy.src.Infrastructure.Configuration;
using DotNetEcosystemStudy.src.Infrastructure.Model;
using DotNetEcosystemStudy.src.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.src.Infrastructure.Context;

public class OrganizationContext : DbContext
{
    protected readonly IGlobalSettings _globalSettings;

    public OrganizationContext(IGlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_globalSettings.Sqlite.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OrganizationDataModel> Organization { get; set; }
}