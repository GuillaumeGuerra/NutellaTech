using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.ViewModels
{
    public class OmniLauncherViewModel : ViewModelBase
    {
        private LaunchersNode _launchers;

        public LaunchersNode Launchers
        {
            get { return _launchers; }
            set
            {
                _launchers = value;
                RaisePropertyChanged();
            }
        }

        public OmniLauncherViewModel()
        {
            Task.Run(() =>
            {
                var xmlConfiguration = new XmlLauncherConfigurationReader().LoadFile("Configuration/Launchers.xml");
                Launchers = new LauncherConfigurationProcessor().ProcessConfiguration(xmlConfiguration);
            });
        }
    }
}
