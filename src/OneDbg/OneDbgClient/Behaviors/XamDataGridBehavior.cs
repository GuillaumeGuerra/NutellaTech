using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Infragistics.Windows.DataPresenter;

namespace OneDbgClient.Behaviors
{
    public class XamDataGridBehavior : Behavior<XamDataGrid>
    {
        public readonly static DependencyProperty SelectedDataItemsProperty
            = DependencyProperty.Register(
                "SelectedDataItems",
                typeof(ObservableCollection<DataRecord>),
                typeof(XamDataGridBehavior),
                new PropertyMetadata());
        public static readonly DependencyProperty SortedFieldsProperty =
            DependencyProperty.Register(
            "SortedFields",
            typeof(ObservableCollection<FieldSortDescription>),
            typeof(XamDataGridBehavior),
            new PropertyMetadata(SortedFieldsPropertyChanged));

        public ObservableCollection<DataRecord> SelectedDataItems
        {
            get { return (ObservableCollection<DataRecord>)GetValue(SelectedDataItemsProperty); }
            set { SetValue(SelectedDataItemsProperty, value); }
        }
        public ObservableCollection<FieldSortDescription> SortedFields
        {
            get { return (ObservableCollection<FieldSortDescription>)GetValue(SortedFieldsProperty); }
            set { SetValue(SortedFieldsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemsChanged += AssociatedObjectOnSelectedItemsChanged;
            AssociatedObjectOnSelectedItemsChanged(AssociatedObject, null);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectedItemsChanged -= AssociatedObjectOnSelectedItemsChanged;
            base.OnDetaching();
        }

        private void AssociatedObjectOnSelectedItemsChanged(object sender, Infragistics.Windows.DataPresenter.Events.SelectedItemsChangedEventArgs e)
        {
            if (SelectedDataItems != null)
            {
                SelectedDataItems.Clear();
                foreach (var selectedDataItem in AssociatedObject.SelectedItems.Records.OfType<DataRecord>())
                {
                    SelectedDataItems.Add(selectedDataItem);
                }
            }
        }

        private static void SortedFieldsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
                (e.OldValue as ObservableCollection<FieldSortDescription>).CollectionChanged -= (dependencyObject as XamDataGridBehavior).OnSortedPropertiesChanged;
            (e.NewValue as ObservableCollection<FieldSortDescription>).CollectionChanged += (dependencyObject as XamDataGridBehavior).OnSortedPropertiesChanged;
        }

        private void OnSortedPropertiesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AssociatedObject.FieldLayouts[0].SortedFields.Clear();
            foreach (var sortedField in SortedFields)
            {
                AssociatedObject.FieldLayouts[0].SortedFields.Add(sortedField);
            }
        }
    }
}
