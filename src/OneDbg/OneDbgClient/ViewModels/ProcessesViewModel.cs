using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infragistics.Windows.DataPresenter;

namespace OneDbgClient.ViewModels
{
    public class ProcessesViewModel : ViewModelBase
    {
        #region Properties

        private string _header = "";
        private ObservableCollection<ProcessViewModel> _allProcesses = new ObservableCollection<ProcessViewModel>();
        private bool _isRefreshAvailable = true;
        private ObservableCollection<DataRecord> _selectedProcesses = new ObservableCollection<DataRecord>();
        private bool _isDebugAvailable = false;

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
        public bool IsRefreshAvailable
        {
            get { return _isRefreshAvailable; }
            set
            {
                _isRefreshAvailable = value;
                RaisePropertyChanged();
            }
        }
        public bool IsDebugAvailable
        {
            get { return _isDebugAvailable; }
            set
            {
                _isDebugAvailable = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<DataRecord> SelectedProcesses
        {
            get { return _selectedProcesses; }
            set
            {
                _selectedProcesses = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RefreshProcessesCommand
        {
            get { return new RelayCommand(RefreshProcesses); }
        }
        public ICommand DebugProcessCommand
        {
            get { return new RelayCommand(DebugProcess); }
        }

        #endregion

        public event Action<ProcessViewModel> OnProcessSelected;

        public ProcessesViewModel()
        {
            SelectedProcesses.CollectionChanged += SelectedProcesses_CollectionChanged;
            RefreshProcesses();
        }

        private void SelectedProcesses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsDebugAvailable = SelectedProcesses.Count > 0;
        }

        private async void RefreshProcesses()
        {
            AllProcesses.Clear();
            var tempProcesses = new List<ProcessViewModel>();
            Header = "Refresh in progress ...";
            IsRefreshAvailable = false;

            await Task.Run(() =>
            {
                foreach (var process in Process.GetProcesses())
                {
                    tempProcesses.Add(new ProcessViewModel(process));
                }
            });

            foreach (var tempProcess in tempProcesses)
            {
                AllProcesses.Add(tempProcess);
            }
            Header = string.Format("{0} processes available for debug", AllProcesses.Count);
            IsRefreshAvailable = true;
        }

        private void DebugProcess()
        {
            foreach (var process in SelectedProcesses)
            {
                var processViewModel = process.DataItem as ProcessViewModel;

                if (processViewModel != null && OnProcessSelected != null)
                    OnProcessSelected(processViewModel);
            }
        }
    }
}
