using OmniLauncher.Services.ConfigurationLoader;

namespace OmniLauncher.Services.CommandLauncher
{
    public interface ICommandLauncher
    {
        bool CanProcess(LauncherCommand command);
        void Execute(LauncherCommand command);
    }
}
