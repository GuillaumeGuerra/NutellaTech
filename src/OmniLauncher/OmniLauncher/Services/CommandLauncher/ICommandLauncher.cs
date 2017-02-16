using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.CommandLauncher
{
    public interface ICommandLauncher
    {
        bool CanProcess(LauncherCommand command);
        void Execute(LauncherCommand command);
    }
}
