using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace OneDbgClient.ViewModels
{
    public class ProcessesViewModel : ViewModelBase
    {
        private string _header = "";
        private ObservableCollection<ProcessViewModel> _allProcesses = new ObservableCollection<ProcessViewModel>();

        public ObservableCollection<ProcessViewModel> AllProcesses
        {
            get { return _allProcesses; }
            set
            {
                _allProcesses = value;
                RaisePropertyChanged();
            }
        }

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RefreshProcessesCommand
        {
            get
            {
                return new RelayCommand(RefreshProcesses);
            }
        }

        private void RefreshProcesses()
        {
            AllProcesses.Clear();
            foreach (var process in Process.GetProcesses())
            {
                AllProcesses.Add(new ProcessViewModel()
                {
                    PID = process.Id,
                    Name = process.ProcessName
                });
            }
        }
    }
}
