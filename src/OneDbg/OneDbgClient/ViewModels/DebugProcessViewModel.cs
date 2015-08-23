using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OneDbgLibrary;

namespace OneDbgClient.ViewModels
{
    public class DebugProcessViewModel : ViewModelBase
    {
        private ProcessViewModel _process;
        private List<RunningThread> _threadStacks;
        private string _threadsSummary = "Please load the thread stacks";

        public ProcessViewModel Process
        {
            get { return _process; }
            set
            {
                _process = value;
                RaisePropertyChanged();
            }
        }
        public List<RunningThread> ThreadStacks
        {
            get { return _threadStacks; }
            set
            {
                _threadStacks = value;
                RaisePropertyChanged();

            }
        }
        public string ThreadsSummary
        {
            get { return _threadsSummary; }
            set
            {
                _threadsSummary = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadStacksCommand
        {
            get { return new RelayCommand(LoadStacks); }
        }

        private async void LoadStacks()
        {
            ThreadsSummary = string.Format("Getting threads ...");

            await Task.Run(() =>
            {
                var inspector = new ThreadStacksInspector(Process.PID);
                ThreadStacks = inspector.LoadStacks();
                ThreadsSummary = string.Format("{0} thread(s) found", ThreadStacks.Count);
            });
        }
    }
}