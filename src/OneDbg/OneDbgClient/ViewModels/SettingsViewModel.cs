using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace OneDbgClient.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _areSettingsVisible;
        public bool AreSettingsVisible
        {
            get { return _areSettingsVisible; }
            set
            {
                _areSettingsVisible = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenSettingsCommand
        {
            get { return new RelayCommand(OpenSettings); }
        }

        public ICommand ILikeStarWarsCommand
        {
            get { return new RelayCommand(ILikeStarWars); }
        }

        private void ILikeStarWars()
        {
            Process.Start("http://starwars.wikia.com/wiki/Main_Page");
        }

        private void OpenSettings()
        {
            AreSettingsVisible = true;
        }
    }
}
