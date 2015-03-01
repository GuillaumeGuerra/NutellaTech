using Framework;
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

namespace KanbanBoard.Behaviors
{
    public class UserStoryAnimationBehavior : AbstractAnimationBehavior<IDraggableItem, Control>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var descriptor = DependencyPropertyDescriptor.FromProperty(DraggableItemViewModel.HightlightStatusProperty, typeof(DraggableItemViewModel));
            descriptor.AddValueChanged(ViewModel, HightlightStatusChanged);
        }

        protected override void MouseEnter(object sender, MouseEventArgs e)
        {
            Position position = Stories.GetPosition(ViewModel.Status, ViewModel.Index);
            double originX = 0D, originY = 0D;
            if (position.Index < Stories.BoardLayout[position.Status].Width)
                originY = 0D;
            else if (position.Index >= Stories.BoardLayout[position.Status].Width * (Stories.BoardLayout.RowsCount - 1))
                originY = 1D;
            else
                originY = 0.5D;

            if (position.Status == Stories.BoardLayout.Columns.First().Status && MathExtensions.Remainder(position.Index, Stories.BoardLayout[position.Status].Width) == 0)
                originX = 0D;
            else if (position.Status == Stories.BoardLayout.Columns.Last().Status && MathExtensions.Remainder(position.Index, Stories.BoardLayout[position.Status].Width) == Stories.BoardLayout[position.Status].Width - 1)
                originX = 1D;
            else
                originX = 0.5D;

            AssociatedObject.RenderTransformOrigin = new System.Windows.Point(originX, originY);
            CheckAndLaunchAnimation("enterStoryboard", e);
        }

        protected override void MouseLeave(object sender, MouseEventArgs e)
        {
            CheckAndLaunchAnimation("leaveStoryboard", e);
        }

        protected override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                CheckAndLaunchAnimation("fadeStoryboard", e);
        }

        protected override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                CheckAndLaunchAnimation("unfadeStoryboard", e);
        }

        private void HightlightStatusChanged(object sender, EventArgs e)
        {
            AssociatedObject.RenderTransformOrigin = new System.Windows.Point(0.5D, 0.5D);
            switch (ViewModel.HightlightStatus)
            {
                case HightlightStatus.Hightlighted:
                    AnimationUtilities.LaunchAnimation(AssociatedObject, "resetItemStoryboard");
                    break;
                case HightlightStatus.Hidden:
                    AnimationUtilities.LaunchAnimation(AssociatedObject, "hideItemStoryboard");
                    break;
            }
        }
    }
}
