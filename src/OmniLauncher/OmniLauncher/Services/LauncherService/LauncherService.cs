using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using OmniLauncher.Services.CommandLauncher;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.MessageService;

namespace OmniLauncher.Services.LauncherService
{
    public class LauncherService : ILauncherService
    {
        public IMessageService MessageService { get; set; }

        public void Launch(LauncherLink launcher)
        {
            try
            {
                var plugins = App.Container.Resolve<IEnumerable<ICommandLauncher>>();

                foreach (var command in launcher.Commands)
                {
                    var plugin = plugins.FirstOrDefault(p => p.CanProcess(command));
                    if (plugin == null)
                        throw new NotSupportedException($"Unable to find a CommandLauncher plugin for the type {command.GetType().FullName}");

                    plugin.Execute(command);
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowException(exception);
            }
        }
    }
}