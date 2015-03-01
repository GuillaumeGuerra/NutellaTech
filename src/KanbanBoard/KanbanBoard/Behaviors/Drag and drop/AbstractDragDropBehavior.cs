using Framework;
using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace KanbanBoard.Behaviors
{
    public abstract class AbstractDragDropBehavior<TViewModel, TControl> : AbstractMouseBehavior<TViewModel, TControl>
        where TViewModel : class
        where TControl : UIElement
    {
        #region Properties

        protected double InitialLeftPos { get; set; }
        protected double InitialTopPos { get; set; }
        protected bool ItemGrabbed { get; set; }

        #endregion

        public AbstractDragDropBehavior(bool handlePreviewEvents)
            : base(handlePreviewEvents) { }

        #region Overrides

        protected override sealed void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DoMouseLeftButtonDown(e) ||
                e.RightButton == MouseButtonState.Pressed && DoMouseRightButtonDown(e) ||
                e.MiddleButton == MouseButtonState.Pressed && DoMouseMiddleButtonDown(e))
            {
                ItemGrabbed = true;
                e.Handled = true;
            }
        }

        protected override sealed void MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed || e.MiddleButton == MouseButtonState.Pressed) &&
                ItemGrabbed && DoMouseMove(e))
                e.Handled = true;
        }

        protected override sealed void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ItemGrabbed && DoMouseLeftButtonUp(e))
            {
                ItemGrabbed = false;
                e.Handled = true;
            }
        }

        protected override sealed void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            DoMouseWheel(e);
        }

        #endregion

        #region Virtual Methods

        protected virtual bool DoMouseLeftButtonDown(MouseButtonEventArgs e) { return false; }

        protected virtual bool DoMouseRightButtonDown(MouseButtonEventArgs e) { return false; }

        protected virtual bool DoMouseMiddleButtonDown(MouseButtonEventArgs e) { return false; }

        protected virtual bool DoMouseMove(MouseEventArgs e) { return false; }

        protected virtual bool DoMouseLeftButtonUp(MouseButtonEventArgs e) { return false; }

        protected virtual bool DoMouseWheel(MouseWheelEventArgs e) { return false; }

        #endregion
    }
}
