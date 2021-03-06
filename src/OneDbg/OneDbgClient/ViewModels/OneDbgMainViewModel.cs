﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using OneDbgClient.Framework;

namespace OneDbgClient.ViewModels
{
    public class OneDbgMainViewModel : CommonViewModel
    {
        private ObservableCollection<DebugProcessViewModel> _debugProcesses = new ObservableCollection<DebugProcessViewModel>();

        public OneDbgMainViewModel()
        {
            App.Current.ViewModelLocator().Processes.OnProcessSelected += OnProcessSelected;
        }

        public ObservableCollection<DebugProcessViewModel> DebugProcesses
        {
            get { return _debugProcesses; }
            set
            {
                _debugProcesses = value;
                RaisePropertyChanged();
            }
        }

        private void OnProcessSelected(ProcessViewModel process)
        {
            DebugProcesses.Add(new DebugProcessViewModel()
            {
                Process = process
            });
        }
    }
}
