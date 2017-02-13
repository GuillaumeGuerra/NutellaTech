using System;
using System.Diagnostics;
using OmniLauncher.Services.IExceptionManager;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.LauncherService
{
    public class LauncherService : ILauncherService
    {
        public IMessageService MessageService { get; set; }

        public void Launch(LauncherLink launcher)
        {
            try
            {
                Process.Start(launcher.Command);
            }
            catch (Exception exception)
            {
                MessageService.ShowException(exception);
            }
        }
    }
}