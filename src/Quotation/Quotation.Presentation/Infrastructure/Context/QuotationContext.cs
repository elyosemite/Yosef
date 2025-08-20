using Quotation.Infrastructure.Settings.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Quotation.Presentation.Infrastructure.Model;
using Quotation.Presentation.Infrastructure.Configuration;

namespace Quotation.Infrastructure.Context;

public class QuotationContext : DbContext
{
    protected readonly IGlobalSettings _globalSettings;
    public static readonly ILoggerFactory EfLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddSerilog();
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
    });

    public QuotationContext(IGlobalSettings globalSettings, DbContextOptions options)
        : base(options)
    {
        _globalSettings = globalSettings;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuotationConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<QuotationDataModel> Quotation { get; set; }
}