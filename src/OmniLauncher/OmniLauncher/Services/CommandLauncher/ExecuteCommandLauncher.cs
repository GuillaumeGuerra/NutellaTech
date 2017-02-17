using System.Diagnostics;

namespace OmniLauncher.Services.CommandLauncher
{
    public class ExecuteCommandLauncher : BaseCommandLauncher<ExecuteCommand>
    {
        protected override void DoExecute(ExecuteCommand command)
        {
            Process.Start(command.Command);
        }
    }
}