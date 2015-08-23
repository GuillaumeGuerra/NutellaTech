using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<DataRecord> SelectedDataItems
        {
            get { return (ObservableCollection<DataRecord>)GetValue(SelectedDataItemsProperty); }
            set { SetValue(SelectedDataItemsProperty, value); }
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
    }
}
