using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarLauncher.Business
{
    [Export(typeof(IEnvironmentDiscoverer))]
    public class EnvironmentDiscoverer : IEnvironmentDiscoverer
    {
        [Import]
        public IDirectoryScanner Scanner { get; set; }

        public List<StarEnvironment> DiscoverEnvironments()
        {
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directories = Directory.GetDirectories("../").Where(d => Path.GetFullPath(d) != currentDir);

            List<StarEnvironment> environments = new List<StarEnvironment>();

            foreach (var directory in directories)
            {
                StarEnvironment environment = Scanner.ScanDirectory(directory);
                if (environment != null)
                    environments.Add(environment);
            }

            return environments;
        }
    }
}
