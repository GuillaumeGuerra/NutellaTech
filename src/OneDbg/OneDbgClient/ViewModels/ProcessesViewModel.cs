using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace OneDbgClient.ViewModels
{
    public class ProcessesViewModel : ViewModelBase
    {
        private string _header = "";
        private ObservableCollection<ProcessViewModel> _allProcesses = new ObservableCollection<ProcessViewModel>();
        private string _selectedProcess;

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

        public string SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
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

        public ICommand DebugProcessCommand
        {
            get
            {
                return new RelayCommand(DebugProcess);
            }
        }

        public event Action<ProcessViewModel> OnProcessSelected;

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

            Header = string.Format("{0} process(es) available for debug", AllProcesses.Count);
        }

        private void DebugProcess()
        {
            int selectedProcessPID = 0;
            if (OnProcessSelected != null && Int32.TryParse(SelectedProcess, out selectedProcessPID))
            {
                var selectedProcess = AllProcesses.SingleOrDefault(process => process.PID == selectedProcessPID);
                if (selectedProcess != null)
                    OnProcessSelected(selectedProcess);
            }
        }
    }
}
