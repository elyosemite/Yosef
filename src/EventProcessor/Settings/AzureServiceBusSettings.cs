using EventProcessor.Settings.Interfaces;

namespace EventProcessor.Settings;

public class AzureServiceBusSettings : IAzureServiceBusSettings
{
    private string _connectionString = string.Empty;
    private string _topicName = string.Empty;
    
    public string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value.Trim('"');
    }

    public string TopicName
    {
        get => _topicName;
        set => _topicName = value.Trim('"');
    }
}
