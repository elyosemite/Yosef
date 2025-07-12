namespace ProjectManagement.Infrastructure.Settings.Interfaces;

public interface IBaseServiceUriSettings
{
    string CloudRegion { get; set; }
    string Vault { get; set; }
    string VaultWithHash { get; }
    string VaultWithHashAndSecretManagerProduct { get; }
    string Api { get; set; }
    string Identity { get; set; }
    string Admin { get; set; }
    string Notifications { get; set; }
    string Sso { get; set; }
    string Scim { get; set; }
    string InternalNotifications { get; set; }
    string InternalAdmin { get; set; }
    string InternalIdentity { get; set; }
    string InternalApi { get; set; }
    string InternalVault { get; set; }
    string InternalSso { get; set; }
    string InternalScim { get; set; }
    string InternalBilling { get; set; }
}