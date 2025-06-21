namespace DotNetEcosystemStudy.Settings.Interfaces;

public interface IRabbitMqSettings
{
    string HostName { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string ExchangeName { get; set; }
}
