using ProjectManagement.Infrastructure.Configuration;
using ProjectManagement.Infrastructure.Model;
using ProjectManagement.Infrastructure.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ProjectManagement.Infrastructure.Context;

public class OrganizationContext : DbContext
{
    protected readonly IGlobalSettings _globalSettings;
    public static readonly ILoggerFactory EfLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddSerilog();
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
    });

    public OrganizationContext(IGlobalSettings globalSettings, DbContextOptions options)
        : base(options)
    {
        _globalSettings = globalSettings;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OrganizationDataModel> Organization { get; set; }
}