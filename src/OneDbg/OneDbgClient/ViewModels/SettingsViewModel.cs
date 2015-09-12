using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infragistics.Windows.Themes;
using OneDbgClient.Framework;

namespace OneDbgClient.ViewModels
{
    public class SettingsViewModel : CommonViewModel
    {
        private bool _areSettingsVisible;
        private string _gridTheme = "Office2010Blue";
        private ObservableCollection<string> _allGridThemes = new ObservableCollection<string>(ThemeManager.GetThemes());

        public bool AreSettingsVisible
        {
            get { return _areSettingsVisible; }
            set
            {
                _areSettingsVisible = value;
                RaisePropertyChanged();
            }
        }
        public string GridTheme
        {
            get { return _gridTheme; }
            set
            {
                _gridTheme = value;
                RaisePropertyChanged();
                ThemeService.ApplyTheme(GridTheme);
            }
        }
        public ObservableCollection<string> AllGridThemes
        {
            get { return _allGridThemes; }
            set
            {
                _allGridThemes = value;
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
