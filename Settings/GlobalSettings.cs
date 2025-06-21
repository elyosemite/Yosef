using System.Text.Json.Serialization;
using DotNetEcosystemStudy.Settings.Interfaces;

namespace DotNetEcosystemStudy.Settings;

public class GlobalSettings : IGlobalSettings
{
    public GlobalSettings()
    {
        BaseServiceUri = new BaseServiceUriSettings(this);
        EventLogging = new EventLoggingSettings();
        UriSettings = new UriSettings(this);
        DistributedIpRateLimiting = new DistributedIpRateLimitingSettings();
        MySql = new SqlSettings();
        SqlServer = new SqlSettings();
        PostgreSql = new SqlSettings();
        Sqlite = new SqlSettings();
        DataProtection = new DataProtectionSettings(this);
        EnableCloudCommunication = false;
    }

    public bool SelfHosted { get; set; }
    public bool UniqueDeployed { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public bool EnableCloudCommunication { get; set; }

    public IBaseServiceUriSettings BaseServiceUri { get; set; }
    public IEventLoggingSettings EventLogging { get; set; }

    [JsonIgnore]
    public IUriSettings UriSettings { get; set; }
    public IDistributedIpRateLimitingSettings DistributedIpRateLimiting { get; set; }
    public ISqlSettings MySql { get; set; }
    public ISqlSettings SqlServer { get; set; }
    public ISqlSettings PostgreSql { get; set; }
    public ISqlSettings Sqlite { get; set; }
    public IDataProtectionSettings DataProtection { get; set; }
}
