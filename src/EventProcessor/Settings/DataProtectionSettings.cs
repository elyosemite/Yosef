using EventProcessor.Settings.Interfaces;

namespace EventProcessor.Settings;

public class DataProtectionSettings : IDataProtectionSettings
{
    private readonly GlobalSettings _globalSettings;

    private string _directory = string.Empty;

    public DataProtectionSettings(GlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    public string CertificateThumbprint { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;
    public string Directory
    {
        get => _globalSettings.UriSettings.BuildDirectory(_directory, "/core/aspnet-dataprotection");
        set => _directory = value;
    }
}