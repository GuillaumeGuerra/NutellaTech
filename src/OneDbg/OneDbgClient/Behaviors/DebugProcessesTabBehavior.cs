using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using OneDbgClient.ViewModels;
using OneDbgClient.Views;

namespace OneDbgClient.Behaviors
{
    public class DebugProcessesTabBehavior : Behavior<TabControl>
    {
        public static readonly DependencyProperty DebugProcessesProperty = DependencyProperty.Register("DebugProcesses", typeof(ObservableCollection<DebugProcessViewModel>), typeof(DebugProcessesTabBehavior), new PropertyMetadata(DebugProcessesChanged));

        private static void DebugProcessesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            (args.NewValue as ObservableCollection<DebugProcessViewModel>).CollectionChanged +=
                (dependencyObject as DebugProcessesTabBehavior).DebugProcesses_CollectionChanged;
        }

        public ObservableCollection<DebugProcessViewModel> DebugProcesses
        {
            get { return (ObservableCollection<DebugProcessViewModel>)GetValue(DebugProcessesProperty); }
            set { SetValue(DebugProcessesProperty, value); }
        }

        private void DebugProcesses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (DebugProcessViewModel newProcess in e.NewItems)
                    {
                        AssociatedObject.Items.Add(PrepareTabForNewProcessToDebug(newProcess));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static TabItem PrepareTabForNewProcessToDebug(DebugProcessViewModel process)
        {
            return new TabItem()
            {
                Header = string.Format("{0} - {1}", process.Process.PID, process.Process.Name),
                DataContext = process,
                Content = new DebugProcessView(),
                IsSelected = true
            };
        }
    }
}
