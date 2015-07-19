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
        private IEnumerable<RunningThread> _threadStacks;
        private List<RunningThread> _allThreadsStacks;
        private string _threadsSummary = "Please load the thread stacks";
        private bool _filterWaitingThreads = false;
        private bool _filterSimilarThreads = false;
        private Visibility _filtersVisibility = Visibility.Collapsed;

        public ProcessViewModel Process
        {
            get { return _process; }
            set
            {
                _process = value;
                RaisePropertyChanged();
            }
        }
        public List<RunningThread> AllThreadStacks
        {
            get { return _allThreadsStacks; }
            set
            {
                _allThreadsStacks = value;
                RaisePropertyChanged();

            }
        }
        public IEnumerable<RunningThread> ThreadStacks
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
        public bool FilterWaitingThreads
        {
            get { return _filterWaitingThreads; }
            set
            {
                _filterWaitingThreads = value;
                RaisePropertyChanged();
                ApplyThreadStacksFilters();
            }
        }
        public bool FilterSimilarThreads
        {
            get { return _filterSimilarThreads; }
            set
            {
                _filterSimilarThreads = value;
                RaisePropertyChanged();
                ApplyThreadStacksFilters();
            }
        }
        public Visibility FiltersVisibility
        {
            get { return _filtersVisibility; }
            set
            {
                _filtersVisibility = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadStacksCommand
        {
            get { return new RelayCommand(LoadStacks); }
        }

        private async void LoadStacks()
        {
            await Task.Run((Action)(() =>
            {
                var inspector = new ThreadStacksInspector(Process.PID);
                AllThreadStacks = inspector.LoadStacks();
                ApplyThreadStacksFilters();
                ThreadsSummary = string.Format("{0} thread(s) found", AllThreadStacks.Count);
                FiltersVisibility = Visibility.Visible;
            }));
        }

        private void ApplyThreadStacksFilters()
        {
            ThreadStacks = AllThreadStacks;
            //if (FilterSimilarThreads)
            //    ThreadStacks = ThreadStacks.Where(t => );
            if (FilterWaitingThreads)
                ThreadStacks = ThreadStacks.Where(t => !t.IsWaiting);
        }
    }
}