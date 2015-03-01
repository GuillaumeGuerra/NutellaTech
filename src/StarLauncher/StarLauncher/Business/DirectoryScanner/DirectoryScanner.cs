using StarLauncher.Configuration;
using StarLauncher.Entities;
using StarLauncher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarLauncher.Business
{
    [Export(typeof(IDirectoryScanner))]
    public class DirectoryScanner : IDirectoryScanner
    {
        public StarEnvironment ScanDirectory(string directory)
        {
            var files = Directory.GetFiles(directory, "starmanifest.xml");
            if (files.Length != 1)
                return null;

            EnvironmentConfiguration conf = GetConfiguration(files[0]);

            return new StarEnvironment()
            {
                Name = conf.Name,
                Image = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/" + conf.Image, UriKind.Absolute),
                ExecutablePath = conf.Executable,
                HomewareRoot = conf.HomewareRoot,
                TargetDirectoryName = Path.Combine(conf.HomewareRoot, conf.DirectoryName ?? conf.Name),
                SourceDirectoryName = Path.GetFullPath(directory)
            };
        }

        private EnvironmentConfiguration GetConfiguration(string path)
        {
            EnvironmentConfiguration conf = new EnvironmentConfiguration();
            conf.Read(path);
            return conf;
        }
    }
}
