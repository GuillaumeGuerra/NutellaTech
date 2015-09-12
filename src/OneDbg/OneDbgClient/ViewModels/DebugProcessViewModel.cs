using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using OneDbgClient.Framework;
using OneDbgLibrary;

namespace OneDbgClient.ViewModels
{
    public class DebugProcessViewModel : CommonViewModel
    {
        #region Properties

        private ProcessViewModel _process;
        private List<RunningThread> _threadStacks;
        private string _threadsSummary = "Please load the thread stacks";
        private bool _areThreadLoaded = false;
        private List<RunningThread> _previousThreadsSnapshot;
        private ObservableCollection<FieldSortDescription> _sortedFields = new ObservableCollection<FieldSortDescription>();
        private Visibility _deltaStateVisibility = Visibility.Hidden;
        private bool _isProgressRingActive = false;

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
        public bool AreThreadLoaded
        {
            get { return _areThreadLoaded; }
            set
            {
                _areThreadLoaded = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<FieldSortDescription> SortedFields
        {
            get { return _sortedFields; }
            set
            {
                _sortedFields = value;
                RaisePropertyChanged();
            }
        }
        public Visibility DeltaStateVisibility
        {
            get { return _deltaStateVisibility; }
            set
            {
                _deltaStateVisibility = value;
                RaisePropertyChanged();
            }
        }
        public bool IsProgressRingActive
        {
            get { return _isProgressRingActive; }
            set
            {
                _isProgressRingActive = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadStacksCommand
        {
            get { return new RelayCommand(LoadStacks); }
        }
        public ICommand GetDeltaStacksCommand
        {
            get { return new RelayCommand(GetDeltaStacks); }
        }
        public ICommand ExportToExcelCommand
        {
            get { return new RelayCommand<XamDataGrid>(ExportToExcel); }
        }
        public ICommand ExportToClipboardCommand
        {
            get { return new RelayCommand(ExportToClipboard); }
        }

        #endregion

        private async void LoadStacks()
        {
            try
            {
                ThreadsSummary = string.Format("Getting threads ...");
                AreThreadLoaded = false;
                DeltaStateVisibility = Visibility.Collapsed;
                IsProgressRingActive = true;

                ThreadStacks = await GetStacks();
                _previousThreadsSnapshot = ThreadStacks;

                SortedFields.Clear();

                ThreadsSummary = string.Format("{0} threads found", ThreadStacks.Count);
                AreThreadLoaded = true;
                IsProgressRingActive = false;
            }
            catch (Exception e)
            {
                MessageService.ShowError("Unable to get stacks", "Make sure the process type (x64/x86) matches the current one", e);
                AreThreadLoaded = false;
                IsProgressRingActive = false;
            }
        }

        private async void GetDeltaStacks()
        {
            try
            {
                ThreadsSummary = "Getting new snapshot of threads ...";
                AreThreadLoaded = false;
                IsProgressRingActive = true;

                var previous = _previousThreadsSnapshot;
                var current = await GetStacks();
                _previousThreadsSnapshot = previous;

                ThreadsSummary = "Computing delta between the two snapshots ...";

                var delta = await Task.Run(() => new ThreadStackDeltaComputer().Compare(previous, current));
                ThreadStacks = delta.StillAliveThreads.Union(delta.NewThreads).Union(delta.TerminatedThreads).ToList();

                ThreadsSummary = string.Format("{0} threads still running, {1} new, {2} terminated ...",
                    delta.StillAliveThreads.Count,
                    delta.NewThreads.Count,
                    delta.TerminatedThreads.Count);

                SortedFields.Clear();
                SortedFields.Add(new FieldSortDescription("DeltaState", ListSortDirection.Ascending, true));

                AreThreadLoaded = true;
                DeltaStateVisibility = Visibility.Visible;
                IsProgressRingActive = false;
            }
            catch (Exception e)
            {
                MessageService.ShowError("Unable to compute delta stacks", e);
                AreThreadLoaded = false;
                IsProgressRingActive = false;
            }
        }

        private Task<List<RunningThread>> GetStacks()
        {
            return Task.Run(() => new ThreadStacksInspector(Process.PID).LoadStacks());
        }

        private void ExportToClipboard()
        {
            try
            {
                var builder = new StringBuilder();
                builder.AppendLine("PID-" + Process.PID + "\tProcess Name-" + Process.Name);
                foreach (var thread in ThreadStacks)
                {
                    builder.AppendLine("ThreadId-" + thread.ThreadId + "\tLockCount-" + thread.LockCount);
                    foreach (var frame in thread.Stack)
                    {
                        builder.AppendLine("\t" + frame.DisplayString);
                    }
                }

                Clipboard.SetText(builder.ToString());
            }
            catch (Exception e)
            {
                MessageService.ShowError("Unable to export to clipboad", "It's possible that access to the clipboard is restricted", e);
            }
        }

        private void ExportToExcel(XamDataGrid dataGrid)
        {
            string uncFileName = null;
            try
            {
                if (!Directory.Exists("Exports"))
                    Directory.CreateDirectory("Exports");

                string fileName = Path.GetFullPath(string.Format("Exports/{0}.xlsx", Process.PID));

                try
                {
                    DataPresenterExcelExporter exporter = new DataPresenterExcelExporter();
                    exporter.Export(dataGrid, fileName, WorkbookFormat.Excel2007, new ExportOptions());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Received exception : " + Environment.NewLine + ex.ToString(),
                        "Unable to export to excel",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                uncFileName = string.Format(@"\\{0}\{1}", Environment.MachineName, fileName.Replace(":", "$"));

                MessageService.ShowInformation("Export completed", string.Format(
                        "Export to excel is successful{0}The UNC path will be stored into your clipboard ({1})", Environment.NewLine, uncFileName));
            }
            catch (Exception e)
            {
                MessageService.ShowError("Unable to export to excel", e);
            }

            try
            {
                Clipboard.SetText(uncFileName);
            }
            catch (Exception e)
            {
                MessageService.ShowError("Unable to export to the clipboard", "It's possible that access to the clipboard is restricted", e);
            }
        }
    }
}