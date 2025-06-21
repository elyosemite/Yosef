using DotNetEcosystemStudy.Settings.Interfaces;

namespace DotNetEcosystemStudy.Settings;

public class SqlSettings : ISqlSettings
{
    private string _connectionString = string.Empty;
    private string _readOnlyConnectionString = string.Empty;
    private string _jobSchedulerConnectionString = string.Empty;
    public bool SkipDatabasePreparation { get; set; }
    public bool DisableDatabaseMaintenanceJobs { get; set; }

    public string ConnectionString
    {
        get => _connectionString;
        set
        {
            Console.WriteLine("Setting ConnectionString to: " + value);
            _connectionString = value.Trim('"');
        }
    }

    public string ReadOnlyConnectionString
    {
        get => string.IsNullOrWhiteSpace(_readOnlyConnectionString) ?
            _connectionString : _readOnlyConnectionString;
        set => _readOnlyConnectionString = value.Trim('"');
    }

    public string JobSchedulerConnectionString
    {
        get => _jobSchedulerConnectionString;
        set => _jobSchedulerConnectionString = value.Trim('"');
    }
}