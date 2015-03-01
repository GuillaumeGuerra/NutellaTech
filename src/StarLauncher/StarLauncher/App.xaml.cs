using StarLauncher.Business;
using StarLauncher.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace StarLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (e.Args.Count() > 0)
            {
                AllocConsole();
                var env = ObjectFactory.GetInstance<IDirectoryScanner>().ScanDirectory(e.Args[0]);
                if (env != null)
                {
                    ObjectFactory.GetInstance<IJumpListManager>().UpdateRecentList(env);
                    ObjectFactory.GetInstance<IEnvironmentLauncher>().LaunchEnvironment(env, new ConsoleObserver());
                }

                Shutdown();
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }
    }
}
