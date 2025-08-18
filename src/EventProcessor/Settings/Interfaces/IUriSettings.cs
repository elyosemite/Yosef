namespace EventProcessor.Settings.Interfaces;

public interface IUriSettings
{
    string BuildDirectory(string explicitValue, string appendedValue);
    string BuildExternalUri(string explicitValue, string name);
}
