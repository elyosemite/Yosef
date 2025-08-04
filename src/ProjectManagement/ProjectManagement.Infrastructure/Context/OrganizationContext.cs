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

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            Log.Information("Configuring OrganizationContext with default options.");
            options.UseLoggerFactory(EfLoggerFactory);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
        else
        {
            Log.Information("OrganizationContext is already configured. Skipping default configuration.");    
        }

        Log.Information("Configuring OrganizationContext with PostgreSQL connection string.");
        if (_globalSettings.PostgreSql == null || string.IsNullOrEmpty(_globalSettings.PostgreSql.ConnectionString))
        {
            throw new InvalidOperationException("PostgreSQL connection string is not configured.");
        }
        Log.Information("Using connection string: {ConnectionString}", _globalSettings.PostgreSql.ConnectionString);
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