using DotNetEcosystemStudy.Settings.Interfaces;

namespace DotNetEcosystemStudy.Settings;

public class RabbitMqSettings : IRabbitMqSettings
{
    private string _hostName = string.Empty;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _exchangeName = string.Empty;

    public string HostName
    {
        get => _hostName;
        set => _hostName = value.Trim('"');
    }
    public string Username
    {
        get => _username;
        set => _username = value.Trim('"');
    }
    public string Password
    {
        get => _password;
        set => _password = value.Trim('"');
    }
    public string ExchangeName
    {
        get => _exchangeName;
        set => _exchangeName = value.Trim('"');
    }
}