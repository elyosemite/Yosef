using ProjectManagement.Infrastructure.Settings.Interfaces;

namespace ProjectManagement.Infrastructure.Settings;

public class BaseServiceUriSettings : IBaseServiceUriSettings
{
    private readonly GlobalSettings _globalSettings;

    private string _api = string.Empty;
    private string _identity = string.Empty;
    private string _admin = string.Empty;
    private string _notifications = string.Empty;
    private string _sso = string.Empty;
    private string _scim = string.Empty;
    private string _internalApi = string.Empty;
    private string _internalIdentity = string.Empty;
    private string _internalAdmin = string.Empty;
    private string _internalNotifications = string.Empty;
    private string _internalSso = string.Empty;
    private string _internalVault = string.Empty;
    private string _internalScim = string.Empty;
    private string _internalBilling = string.Empty;

    public BaseServiceUriSettings(GlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    public string CloudRegion { get; set; } = string.Empty;
    public string Vault { get; set; } = string.Empty;
    public string VaultWithHash => $"{Vault}#";

    public string VaultWithHashAndSecretManagerProduct => $"{Vault}#sm";

    public string Api
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_api, "api");
        set => _api = value;
    }
    public string Identity
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_identity, "identity");
        set => _identity = value;
    }
    public string Admin
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_admin, "admin");
        set => _admin = value;
    }
    public string Notifications
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_notifications, "notifications");
        set => _notifications = value;
    }
    public string Sso
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_sso, "sso");
        set => _sso = value;
    }
    public string Scim
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_scim, "scim");
        set => _scim = value;
    }
    public string InternalNotifications
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalNotifications, "notifications");
        set => _internalNotifications = value;
    }
    public string InternalAdmin
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalAdmin, "admin");
        set => _internalAdmin = value;
    }
    public string InternalIdentity
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalIdentity, "identity");
        set => _internalIdentity = value;
    }
    public string InternalApi
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalApi, "api");
        set => _internalApi = value;
    }
    public string InternalVault
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalVault, "web");
        set => _internalVault = value;
    }
    public string InternalSso
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalSso, "sso");
        set => _internalSso = value;
    }
    public string InternalScim
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalScim, "scim");
        set => _internalScim = value;
    }
    public string InternalBilling
    {
        get => _globalSettings.UriSettings.BuildExternalUri(_internalBilling, "billing");
        set => _internalBilling = value;
    }
}
