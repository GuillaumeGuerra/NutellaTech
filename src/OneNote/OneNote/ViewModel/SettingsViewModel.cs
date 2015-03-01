using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            UserName = Environment.UserName;
            AreSettingsVisible = false;
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private bool _areSettingsVisible;
        public bool AreSettingsVisible
        {
            get { return _areSettingsVisible; }
            set
            {
                _areSettingsVisible = value;
                RaisePropertyChanged("AreSettingsVisible");
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
