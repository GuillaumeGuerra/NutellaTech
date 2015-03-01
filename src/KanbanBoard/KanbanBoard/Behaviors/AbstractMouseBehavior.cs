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
    public abstract class AbstractMouseBehavior<TViewModel, TControl> : Behavior<TControl>
        where TViewModel : class
        where TControl : UIElement
    {
        #region Properties

        public static readonly DependencyProperty StoriesProperty =
            DependencyProperty.Register("Stories", typeof(UserStoriesViewModel), typeof(AbstractMouseBehavior<TViewModel, TControl>));

        public UserStoriesViewModel Stories
        {
            get { return (UserStoriesViewModel)GetValue(StoriesProperty); }
            set { SetValue(StoriesProperty, value); }
        }

        private bool HandlePreviewEvents { get; set; }
        protected TViewModel ViewModel
        {
            get
            {
                if (AssociatedObject is Control)
                    return (AssociatedObject as Control).DataContext as TViewModel;
                else
                    return null;
            }
        }

        #endregion

        public AbstractMouseBehavior(bool handlePreviewEvents)
        {
            HandlePreviewEvents = handlePreviewEvents;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AddHandler(HandlePreviewEvents ? Mouse.PreviewMouseDownEvent : Mouse.MouseDownEvent, new MouseButtonEventHandler(MouseDown));
            AssociatedObject.AddHandler(HandlePreviewEvents ? Mouse.PreviewMouseUpEvent : Mouse.MouseUpEvent, new MouseButtonEventHandler(MouseUp));
            AssociatedObject.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(MouseEnter));
            AssociatedObject.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(MouseLeave));
            AssociatedObject.AddHandler(HandlePreviewEvents ? Mouse.PreviewMouseMoveEvent : Mouse.MouseMoveEvent, new MouseEventHandler(MouseMove));
            AssociatedObject.AddHandler(HandlePreviewEvents ? Mouse.PreviewMouseWheelEvent : Mouse.MouseWheelEvent, new MouseWheelEventHandler(MouseWheel));
        }

        #region Virtual Methods

        protected virtual void MouseDown(object sender, MouseButtonEventArgs e) { }

        protected virtual void MouseUp(object sender, MouseButtonEventArgs e) { }

        protected virtual void MouseEnter(object sender, MouseEventArgs e) { }

        protected virtual void MouseLeave(object sender, MouseEventArgs e) { }

        protected virtual void MouseWheel(object sender, MouseWheelEventArgs e) { }

        protected virtual void MouseMove(object sender, MouseEventArgs e) { }

        #endregion
    }
}
