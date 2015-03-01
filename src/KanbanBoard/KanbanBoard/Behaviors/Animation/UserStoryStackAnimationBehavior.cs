using Framework;
using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace KanbanBoard.Behaviors
{
    public class UserStoryStackAnimationBehavior : UserStoryAnimationBehavior
    {
        protected override void MouseEnter(object sender, MouseEventArgs e)
        {
            base.MouseEnter(sender, e);
            LaunchButtonsAnimation("showButtons",e);
        }

        protected override void MouseLeave(object sender, MouseEventArgs e)
        {
            base.MouseLeave(sender, e);

            LaunchButtonsAnimation("hideButtons",e);
        }

        private void LaunchButtonsAnimation(string storyboardName, MouseEventArgs e)
        {
            if (ShouldApplyAnimation())
            {
                Storyboard storyboard = ((AssociatedObject as AdornedControl).Content as Control).FindResource(storyboardName) as Storyboard;
                storyboard.Begin(AssociatedObject);
            }
            else
                e.Handled = true;
        }
    }
}
