using OneNote.View;
using OneNote.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace OneNote.Behaviors
{
    public class TabControlBehavior : Behavior<TabControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        public ChosenTopicsViewModel ChosenTopics
        {
            get { return (ChosenTopicsViewModel)GetValue(ChosenTopicsProperty); }
            set { SetValue(ChosenTopicsProperty, value); }
        }

        private void ChosenTopics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TopicNotificationsViewModel newItem in e.NewItems)
                    {
                        AssociatedObject.Items.Insert(1, new TabItem() { Content = new TopicNotificationsView(), Header = newItem.Topic.Topic.Name, DataContext = newItem });
                    }
                    AssociatedObject.SelectedIndex = 0;
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TopicNotificationsViewModel deletedItem in e.OldItems)
                    {
                        foreach (TabItem tab in AssociatedObject.Items)
                        {
                            if (deletedItem.Equals(tab.DataContext))
                            {
                                AssociatedObject.Items.Remove(tab);
                                break;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public static readonly DependencyProperty ChosenTopicsProperty =
            DependencyProperty.Register("ChosenTopics", typeof(ChosenTopicsViewModel), typeof(TabControlBehavior), new PropertyMetadata(null, ChosenTopicsChanged));

        private static void ChosenTopicsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
                (e.OldValue as ChosenTopicsViewModel).ChosenTopics.CollectionChanged -= (d as TabControlBehavior).ChosenTopics_CollectionChanged;

            if (e.NewValue != null)
                (e.NewValue as ChosenTopicsViewModel).ChosenTopics.CollectionChanged += (d as TabControlBehavior).ChosenTopics_CollectionChanged;
        }
    }
}
