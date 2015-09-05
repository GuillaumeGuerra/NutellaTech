using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OneDbgLibrary;

namespace OneDbgClient.ViewModels
{
    public class DebugProcessViewModel : ViewModelBase
    {
        #region Properties

        private ProcessViewModel _process;
        private List<RunningThread> _threadStacks;
        private string _threadsSummary = "Please load the thread stacks";
        private bool _areThreadLoaded = false;

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
            ThreadsSummary = string.Format("Getting threads ...");
            AreThreadLoaded = false;

            ThreadStacks = await GetStacks();

            ThreadsSummary = string.Format("{0} threads found", ThreadStacks.Count);
            AreThreadLoaded = true;
        }

        private async void GetDeltaStacks()
        {
            var previous = ThreadStacks;
            var current = await GetStacks();
        }

        private async Task<List<RunningThread>> GetStacks()
        {
            return await Task.Run(() => new ThreadStacksInspector(Process.PID).LoadStacks());
        }

        private void ExportToClipboard()
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

        private void ExportToExcel(XamDataGrid dataGrid)
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

            var uncFileName = string.Format(@"\\{0}\{1}", Environment.MachineName, fileName.Replace(":", "$"));

            Clipboard.SetText(uncFileName);
            MessageBox.Show(string.Format("Export to excel is successful, the UNC path has been copied into your clipboard ({0})", uncFileName),
                "Export completed", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}