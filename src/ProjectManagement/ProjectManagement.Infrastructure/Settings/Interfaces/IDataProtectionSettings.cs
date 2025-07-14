namespace ProjectManagement.Infrastructure.Settings.Interfaces;

public interface IDataProtectionSettings
{
    string CertificateThumbprint { get; set; }
    string CertificatePassword { get; set; }
    string Directory { get; set; }
}