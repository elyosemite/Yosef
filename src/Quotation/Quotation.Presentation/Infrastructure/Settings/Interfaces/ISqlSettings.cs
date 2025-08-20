namespace Quotation.Infrastructure.Settings.Interfaces;

public interface ISqlSettings
{
    string ConnectionString { get; set; }
    string ReadOnlyConnectionString { get; set; }
    string JobSchedulerConnectionString { get; set; }
}