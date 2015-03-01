using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace KanbanBoard.Behaviors
{
    public class UserStoryStackDragDropBehavior : UserStoryDragDropBehavior
    {
        protected new UserStoryStackViewModel ViewModel { get { return base.ViewModel as UserStoryStackViewModel; } }

        protected override bool DoMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
                return false;

            if (e.Delta > 0)
                ViewModel.SwitchToNextUserStory();
            else
                ViewModel.SwitchToPreviousUserStory();

            e.Handled = true;

            return true;
        }
    }
}
