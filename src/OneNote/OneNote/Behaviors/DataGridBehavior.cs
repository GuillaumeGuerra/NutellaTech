using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace OneNote.Behaviors
{
    public class DataGridBehavior : Behavior<DataGrid>
    {
        public ICommand RowDoubleClickCommand
        {
            get { return (ICommand)GetValue(RowDoubleClickCommandProperty); }
            set { SetValue(RowDoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty RowDoubleClickCommandProperty =
            DependencyProperty.Register("RowDoubleClickCommand", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
        }

        public void AssociatedObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AssociatedObject != null && AssociatedObject.SelectedItems != null && AssociatedObject.SelectedItems.Count == 1)
            {
                DataGridRow row = AssociatedObject.ItemContainerGenerator.ContainerFromItem(AssociatedObject.SelectedItem) as DataGridRow;
                if (row != null && RowDoubleClickCommand != null)
                    RowDoubleClickCommand.Execute(row);
            }
        }
    }
}
