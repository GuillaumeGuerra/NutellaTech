using Framework.Extensions;
using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using KanbanBoard.Views;
using System.Windows.Interactivity;
using System.Windows;
using Framework;

namespace KanbanBoard.Behaviors
{
    public class DraggableItemEditAnimationBehavior : Behavior<DraggableItemEditView>
    {
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(Visibility), typeof(DraggableItemEditAnimationBehavior), new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(VisiblityChanged)));

        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        private static void VisiblityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Visibility visibility = (Visibility)e.NewValue;
            DraggableItemEditAnimationBehavior behavior = d as DraggableItemEditAnimationBehavior;

            switch (visibility)
            {
                case Visibility.Collapsed:
                    behavior.HideEditor();
                    break;
                case Visibility.Hidden:
                    behavior.HideEditor();
                    break;
                case Visibility.Visible:
                    behavior.ShowEditor();
                    break;
                default:
                    break;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        private void ShowEditor()
        {
            Grid grid = AssociatedObject.FindName("grid") as Grid;
            var storyboard = grid.FindResource("showEditorAnimation") as Storyboard;
            AnimationUtilities.LaunchAnimation(grid, storyboard);
        }

        private void HideEditor()
        {
            Grid grid = AssociatedObject.FindName("grid") as Grid;
            var storyboard = grid.FindResource("hideEditorAnimation") as Storyboard;
            AnimationUtilities.LaunchAnimation(grid, storyboard);
        }
    }
}
