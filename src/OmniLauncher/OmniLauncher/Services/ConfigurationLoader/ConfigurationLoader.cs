using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OmniLauncher.Framework;
using OmniLauncher.Services.CommandLauncher;

namespace OmniLauncher.Services.ConfigurationLoader
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public IEnumerable<LaunchersNode> LoadConfiguration(string path)
        {
            var plugins = App.Container.GetImplementations<ILauncherConfigurationProcessor>();

            foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                var plugin = plugins.FirstOrDefault(p => p.CanProcess(file));

                if (plugin != null)
                    yield return plugin.Load(file);
            }
        }
    }
}