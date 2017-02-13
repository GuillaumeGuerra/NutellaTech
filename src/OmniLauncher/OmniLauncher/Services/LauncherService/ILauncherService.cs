using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.LauncherService
{
    public interface ILauncherService
    {
        void Launch(LauncherLink launcher);
    }
}