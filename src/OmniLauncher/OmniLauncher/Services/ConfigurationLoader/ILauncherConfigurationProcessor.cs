namespace OmniLauncher.Services.ConfigurationLoader
{
    public interface ILauncherConfigurationProcessor
    {
        bool CanProcess(string path);

        LaunchersNode Load(string path);
    }
}