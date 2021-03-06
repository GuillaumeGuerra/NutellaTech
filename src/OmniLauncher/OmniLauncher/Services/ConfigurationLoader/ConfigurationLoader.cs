﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OmniLauncher.Framework;
using OmniLauncher.Services.CommandLauncher;

namespace OmniLauncher.Services.ConfigurationLoader
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public IEnumerable<ILauncherConfigurationProcessor> AllConfigurationProcessors { get; set; }

        public IEnumerable<LaunchersNode> LoadConfiguration(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                var plugin = AllConfigurationProcessors.FirstOrDefault(p => p.CanProcess(file));

                if (plugin != null)
                    yield return plugin.Load(file);
            }
        }
    }
}