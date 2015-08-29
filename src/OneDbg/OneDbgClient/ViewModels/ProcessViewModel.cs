using System;
using System.ComponentModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace OneDbgClient.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        private int _pid;
        private string _name;

        public int PID
        {
            get { return _pid; }
            set
            {
                _pid = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public string ProcessType { get; set; }

        public ProcessViewModel(Process process)
        {
            ReadProcess(process);
        }

        private void ReadProcess(Process process)
        {
            PID = process.Id;
            Name = process.ProcessName;
            ProcessType = process.VirtualMemorySize == process.VirtualMemorySize64 ? "x86" : "x64";
        }
    }
}