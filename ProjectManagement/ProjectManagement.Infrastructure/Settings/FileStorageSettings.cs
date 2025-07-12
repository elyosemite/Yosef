using ProjectManagement.Infrastructure.Settings.Interfaces;

namespace ProjectManagement.Infrastructure.Settings;

public class FileStorageSettings : IFileStorageSettings
{
    private readonly GlobalSettings _globalSettings;
    private readonly string _urlName;
    private readonly string _directoryName;
    private string _connectionString = string.Empty;
    private string _baseDirectory = string.Empty;
    private string _baseUrl = string.Empty;

    public FileStorageSettings(GlobalSettings globalSettings, string urlName, string directoryName)
    {
        _globalSettings = globalSettings;
        _urlName = urlName;
        _directoryName = directoryName;
    }


    public string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value.Trim('"');
    }
    public string BaseDirectory
    {
        get => _globalSettings.UriSettings.BuildDirectory(_baseDirectory, string.Concat("/core/", _directoryName));
        set => _baseDirectory = value;
    }
    public string BaseUrl
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_baseUrl, _urlName);
        set => _baseUrl = value;
    }
}