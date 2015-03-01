using Infragistics.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;

namespace StarLauncher.Views
{
    public class SelectedItemBehavior : Behavior<XamCarouselListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int offset = Math.Min(AssociatedObject.Items.Count, AssociatedObject.ViewSettings.ItemsPerPage) / 2;
            AssociatedObject.ScrollInfo.SetVerticalOffset(
                             AssociatedObject.SelectedIndex - offset);
        }
    }
}
