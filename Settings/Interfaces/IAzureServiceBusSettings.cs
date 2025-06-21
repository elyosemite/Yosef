namespace DotNetEcosystemStudy.Settings.Interfaces;

public interface IAzureServiceBusSettings
{
    public string ConnectionString { get; set; }
    public string TopicName { get; set; }
}
