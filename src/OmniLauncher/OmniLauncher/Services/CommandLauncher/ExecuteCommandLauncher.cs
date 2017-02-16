using System.Diagnostics;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.CommandLauncher
{
    public class ExecuteCommandLauncher : ICommandLauncher
    {
        public bool CanProcess(LauncherCommand command)
        {
            return command is ExecuteCommand;
        }

        public void Execute(LauncherCommand command)
        {
            Process.Start(((ExecuteCommand)command).Command);
        }
    }
}