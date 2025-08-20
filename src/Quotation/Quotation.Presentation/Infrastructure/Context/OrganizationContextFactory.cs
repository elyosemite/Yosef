using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quotation.Infrastructure.Settings;
using Serilog;

namespace Quotation.Infrastructure.Context;

public class QuotationContextFactory : IDesignTimeDbContextFactory<QuotationContext>
{
    public QuotationContext CreateDbContext(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        var configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .Build();

        var globalSettings = new GlobalSettings();
        ConfigurationBinder.Bind(configuration.GetSection("GlobalSettings"), globalSettings);
        Log.Information("Connection string: {@ConnectionString}", globalSettings.PostgreSql.ConnectionString);

        var optionsBuilder = new DbContextOptionsBuilder<QuotationContext>();
        
        optionsBuilder.UseNpgsql(globalSettings.PostgreSql.ConnectionString);
        optionsBuilder.UseLoggerFactory(QuotationContext.EfLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new QuotationContext(globalSettings, optionsBuilder.Options);
    }
}