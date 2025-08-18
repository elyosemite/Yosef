namespace EventProcessor.Settings.Interfaces;

public interface IFileStorageSettings
{
    string ConnectionString { get; set; }
    string BaseDirectory { get; set; }
    string BaseUrl { get; set; }
}