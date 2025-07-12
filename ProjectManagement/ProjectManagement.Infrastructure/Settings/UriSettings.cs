using ProjectManagement.Infrastructure.Settings.Interfaces;

namespace ProjectManagement.Infrastructure.Settings;

public class UriSettings : IUriSettings
{
    private readonly IGlobalSettings _globalSettings;

    public UriSettings(IGlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    public string BuildDirectory(string explicitValue, string appendedValue)
    {
        if (string.IsNullOrEmpty(explicitValue))
        {
            return explicitValue;
        }

        if (!_globalSettings.SelfHosted)
        {
            return string.Empty;
        }

        return string.Concat("etc/myorg", appendedValue);
    }

    public string BuildExternalUri(string explicitValue, string name)
    {
        if (!string.IsNullOrEmpty(explicitValue))
        {
            return explicitValue;
        }

        if (!_globalSettings.SelfHosted)
        {
            return string.Empty;
        }

        return string.Format("{0}/{1}", _globalSettings.BaseServiceUri.Vault, name);
    }
}
