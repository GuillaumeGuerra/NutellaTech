using Framework.Extensions;
using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using KanbanBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace KanbanBoard.Behaviors
{
    public class BoardDragDropBehavior : AbstractDragDropBehavior<WhiteBoardViewModel, ScrollViewer>
    {
        #region Properties

        private Canvas _canvas;

        public Canvas Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    var control = AssociatedObject.FindName("itemsControl") as Control;
                    _canvas = control.Template.FindName("myCanvas", control) as Canvas;
                }

                return _canvas;
            }
        }

        #endregion

        public BoardDragDropBehavior()
            : base(true) { }

        #region Overrides

        protected override bool DoMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            SetLastPosition(e);

            return ShouldProcessEvent(e);
        }

        protected override bool DoMouseMiddleButtonDown(MouseButtonEventArgs e)
        {
            SetLastPosition(e);

            return ShouldProcessEvent(e);
        }

        protected override bool DoMouseMove(MouseEventArgs e)
        {
            Point currentPoint = e.MouseDevice.GetPosition(AssociatedObject);
            var deltaX = InitialLeftPos - currentPoint.X;
            var deltaY = InitialTopPos - currentPoint.Y;

            AssociatedObject.ScrollToHorizontalOffset(AssociatedObject.HorizontalOffset + deltaX);
            AssociatedObject.ScrollToVerticalOffset(AssociatedObject.VerticalOffset + deltaY);

            SetLastPosition(e);

            return true;
        }

        protected override bool DoMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            return true;
        }

        protected override bool DoMouseWheel(MouseWheelEventArgs e)
        {
            if (!ShouldProcessEvent(e) ||
                e.Handled || e.Delta < 0 && ViewModel.BoardDragDrop.BoardZoomRatio <= ViewModel.BoardDragDrop.GetFullScreenZoomRatio())
                return false;

            ViewModel.BoardDragDrop.BoardZoomRatio += e.Delta / 1500D;

            e.Handled = true;

            return true;
        }

        #endregion

        private bool ShouldProcessEvent(MouseEventArgs e)
        {
            DependencyObject current = VisualTreeHelper.HitTest(AssociatedObject, e.GetPosition(AssociatedObject)).VisualHit;

            while (current != null)
            {
                if (current is ScrollBar)
                    return false; // the click has been made in the left or bottom scrollbar, we'll ignore it
                if (current is DraggableItemView && e.MiddleButton == MouseButtonState.Released)
                    return false; // there is an item below the cursor, we'll ignore it
                if (current == AssociatedObject)
                    return true; // we went up in the visual tree to the parent ScrollViewer, so we didn't click on an item card, we can process the click

                current = VisualTreeHelper.GetParent(current);
            }

            return false; // We don't really know where did the click happen, we'll ignore it
        }

        private void SetLastPosition(MouseEventArgs e)
        {
            var point = e.GetPosition(AssociatedObject);
            InitialLeftPos = point.X;
            InitialTopPos = point.Y;
        }
    }
}
