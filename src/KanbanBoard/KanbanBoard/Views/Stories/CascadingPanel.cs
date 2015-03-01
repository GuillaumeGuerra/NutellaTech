using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KanbanBoard.Views
{
    public class CascadingPanel : Panel
    {
        public double ItemOffset
        {
            get { return (double)GetValue(ItemOffsetProperty); }
            set { SetValue(ItemOffsetProperty, value); }
        }

        public static readonly DependencyProperty ItemOffsetProperty =
          DependencyProperty.Register("ItemOffset", typeof(double), typeof(CascadingPanel));

        protected override Size MeasureOverride(Size availableSize)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return availableSize;

            DraggableItemViewModel vm = this.DataContext as DraggableItemViewModel;

            var desiredWidth = vm.Width - ItemOffset * (Children.Count - 1);
            var desiredHeight = vm.Height - ItemOffset * (Children.Count - 1);
            //TODO : review the upper formula => the items are not squared, but rectangles, so we can't remove the same size to both dimensions, we have to apply a ratio between height and width
            Size chosenSize = new Size(desiredWidth, desiredHeight);
            foreach (UIElement child in Children)
            {
                child.Measure(chosenSize);
            }

            return new Size(vm.Width, vm.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return finalSize;

            DraggableItemViewModel vm = this.DataContext as DraggableItemViewModel;

            var desiredWidth = vm.Width - ItemOffset * (Children.Count - 1);
            var desiredHeight = vm.Height - ItemOffset * (Children.Count - 1);
            for (var i = 1; i <= Children.Count; i++)
            {
                var child = Children[Children.Count - i];
                child.Arrange(new Rect(ItemOffset * (i - 1), ItemOffset * (i - 1), desiredWidth, desiredHeight));
            }

            return finalSize;
        }
    }
}
