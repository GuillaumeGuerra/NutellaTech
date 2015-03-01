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
using System.Windows.Media.Animation;

namespace KanbanBoard.Behaviors
{
    public abstract class AbstractAnimationBehavior<TViewModel, TControl> : AbstractMouseBehavior<TViewModel, TControl>
        where TViewModel : class,IDraggableItem
        where TControl : Control
    {
        public AbstractAnimationBehavior()
            : base(false) { }

        protected bool ShouldApplyAnimation()
        {
            return ViewModel != null && ViewModel.IsReadOnly;
        }

        protected void CheckAndLaunchAnimation(string storyboardName, MouseEventArgs e)
        {
            if (ShouldApplyAnimation())
                AnimationUtilities.LaunchAnimation(AssociatedObject, storyboardName);
            else
                e.Handled = true;
        }
    }
}
