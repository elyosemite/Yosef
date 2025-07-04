namespace DotNetEcosystemStudy.src.Settings.Interfaces;

public interface IGlobalSettings
{
    bool SelfHosted { get; set; }
    bool UniqueDeployed { get; set; }
    string ProjectName { get; set; }
    bool EnableCloudCommunication { get; set; }
    IBaseServiceUriSettings BaseServiceUri { get; set; }
    IEventLoggingSettings EventLogging { get; set; }
    IUriSettings UriSettings { get; set; }
    IDistributedIpRateLimitingSettings DistributedIpRateLimiting { get; set; }
    ISqlSettings MySql { get; set; }
    ISqlSettings SqlServer { get; set; }
    ISqlSettings PostgreSql { get; set; }
    ISqlSettings Sqlite { get; set; }
    IDataProtectionSettings DataProtection { get; set; }
}