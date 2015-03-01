using GalaSoft.MvvmLight.Command;
using StarLauncher.Business;
using StarLauncher.Entities;
using StarLauncher.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace StarLauncher.ViewModels
{
    public class EnvironmentsViewModel : DependencyObject
    {
        public static readonly DependencyProperty EnvironmentsProperty =
            DependencyProperty.Register("Environments", typeof(List<StarEnvironment>), typeof(EnvironmentsViewModel));
        public static readonly DependencyProperty SelectedEnvironmentProperty =
            DependencyProperty.Register("SelectedEnvironment", typeof(StarEnvironment), typeof(EnvironmentsViewModel), new PropertyMetadata(null));
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(EnvironmentsViewModel));

        [Import]
        public IEnvironmentDiscoverer Discoverer { get; set; }
        [Import]
        public IEnvironmentLauncher Launcher { get; set; }
        [Import]
        public IJumpListManager JumpListManager { get; set; }

        public ICommand WindowLoadedCommand
        {
            get { return new RelayCommand(InitializeEnvironments); }
        }
        public ICommand EnvironmentLaunchRequestedCommand
        {
            get { return new RelayCommand<StarEnvironment>(EnvironmentLaunchRequested); }
        }

        public List<StarEnvironment> Environments
        {
            get { return (List<StarEnvironment>)GetValue(EnvironmentsProperty); }
            set { SetValue(EnvironmentsProperty, value); }
        }
        public StarEnvironment SelectedEnvironment
        {
            get { return (StarEnvironment)GetValue(SelectedEnvironmentProperty); }
            set { SetValue(SelectedEnvironmentProperty, value); }
        }
        public string ProgressText
        {
            get { return (string)GetValue(ProgressTextProperty); }
            set { SetValue(ProgressTextProperty, value); }
        }

        public EnvironmentsViewModel()
        {
            ObjectFactory.ProcessDependencies(this);
        }

        private void InitializeEnvironments()
        {
            Environments = Discoverer.DiscoverEnvironments();
            SelectedEnvironment = Environments.FirstOrDefault(e => e.Name == "Kanban");
            JumpListManager.UpdateJumpList(Environments);
        }

        private void EnvironmentLaunchRequested(StarEnvironment environment)
        {
            ProgressText = string.Empty;

            JumpListManager.UpdateRecentList(environment);

            var observer = new AppenderObserver();

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            observer.Worker = worker;
            worker.DoWork += (o, e) => Launcher.LaunchEnvironment(environment, observer);
            worker.ProgressChanged += (o, e) => ProgressText += Environment.NewLine + e.UserState as string;

            worker.RunWorkerAsync();
        }
    }
}
