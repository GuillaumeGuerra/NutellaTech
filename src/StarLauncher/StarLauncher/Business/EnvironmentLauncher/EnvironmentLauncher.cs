using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    [Export(typeof(IEnvironmentLauncher))]
    public class EnvironmentLauncher : IEnvironmentLauncher
    {
        [Import]
        public IFileCopier Copier { get; set; }

        public void LaunchEnvironment(StarEnvironment environment, IObserver observer)
        {
            Copier.CopyFiles(environment, observer);

            var process = new Process();

            observer.PushMessage(string.Format("Launching application {0} ({1}) ...", environment.Name, environment.ExecutablePath), MessageLevel.Information);

            process.StartInfo.FileName = environment.ExecutablePath;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.WorkingDirectory = environment.TargetDirectoryName;
            process.Start();
        }
    }
}
