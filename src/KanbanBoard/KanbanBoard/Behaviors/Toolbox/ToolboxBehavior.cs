using Framework;
using Framework.Extensions;
using KanbanBoard.Entities;
using KanbanBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace KanbanBoard.ViewModels
{
    public class ToolboxBehavior : SimpleAdornerBehavior
    {
        public static readonly DependencyProperty ToolboxItemProperty =
            DependencyProperty.Register("ToolboxItem", typeof(IToolboxItem), typeof(ToolboxBehavior));

        public IToolboxItem ToolboxItem
        {
            get { return (IToolboxItem)GetValue(ToolboxItemProperty); }
            set { SetValue(ToolboxItemProperty, value); }
        }

        protected override void BindAdornerProperties()
        {
            base.BindAdornerProperties();

            BindProperty(ToolboxBehavior.ToolboxItemProperty, ToolboxAdorner.ToolboxItemProperty);
            BindProperty(SimpleAdornerBehavior.IsActiveProperty, ToolboxAdorner.IsActiveProperty);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AddHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(PreviewMouseDown));

            //var t = Application.Current.Windows;
            foreach (var window in Application.Current.Windows)
            {
                if(window is WhiteBoardView)
                {
                    AttachedElement = window as Window;
                    return;
                }
            }
            //AttachedElement = VisualTreeHelperExtensions.GetParent<Window>(AssociatedObject);
        }

        private void PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolboxItem = AssociatedObject.DataContext as AvatarViewModel;
            IsActive = true;
        }
    }
}
