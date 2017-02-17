using OmniLauncher.Services.ConfigurationLoader;

namespace OmniLauncher.Services.LauncherService
{
    public interface ILauncherService
    {
        void Launch(LauncherLink launcher);
    }
}