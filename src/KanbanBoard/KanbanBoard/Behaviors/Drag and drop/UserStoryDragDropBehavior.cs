using Framework.Extensions;
using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace KanbanBoard.Behaviors
{
    public class UserStoryDragDropBehavior : AbstractDragDropBehavior<IDraggableItem, Control>
    {
        private int InitialStoryLeft { get; set; }
        private int InitialStoryTop { get; set; }
        private Position LastGridPosition { get; set; }
        private Position InitialGridPosition { get; set; }
        private BlockingStoriesManager BlockingStories { get; set; }

        #region Overrides

        public UserStoryDragDropBehavior()
            : base(false) { }

        protected override bool DoMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Canvas canvas = VisualTreeHelperExtensions.GetParent<Canvas>(AssociatedObject);

            if (!ViewModel.IsReadOnly)
                return false;

            InitialLeftPos = e.GetPosition(canvas).X;
            InitialTopPos = e.GetPosition(canvas).Y;
            InitialStoryLeft = ViewModel.Left;
            InitialStoryTop = ViewModel.Top;

            LastGridPosition = Stories.GetPosition(ViewModel.Status, ViewModel.Index);
            InitialGridPosition = Stories.GetPosition(ViewModel.Status, ViewModel.Index);
            BlockingStories = new BlockingStoriesManager(Stories, ViewModel.Status, ViewModel.Index);

            Stories.StoriesMatrix[LastGridPosition.Status][LastGridPosition.Index] = null;

            return true;
        }

        protected override bool DoMouseMove(MouseEventArgs e)
        {
            Canvas canvas = VisualTreeHelperExtensions.GetParent<Canvas>(AssociatedObject);

            if (!ViewModel.IsReadOnly)
                return false;

            int actualLeft = InitialStoryLeft + Convert.ToInt32(e.GetPosition(canvas).X - InitialLeftPos);
            int actualTop = InitialStoryTop + Convert.ToInt32(e.GetPosition(canvas).Y - InitialTopPos);

            ViewModel.Left = actualLeft;
            ViewModel.Top = actualTop;

            Position currentGridPosition = Stories.GetPosition(Convert.ToInt32(e.GetPosition(canvas).Y), Convert.ToInt32(e.GetPosition(canvas).X));
            if (!currentGridPosition.Equals(LastGridPosition) && currentGridPosition.IsValidPosition &&
                (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (object.ReferenceEquals(Stories.StoriesMatrix[LastGridPosition.Status][LastGridPosition.Index], ViewModel))
                    Stories.StoriesMatrix[LastGridPosition.Status][LastGridPosition.Index] = null;

                List<StoryMove> moves = BlockingStories.PrepareMoves(currentGridPosition.Status, currentGridPosition.Index);
                if (moves == null)
                    return true;

                Stories.MoveStories(moves);

                ViewModel.Status = currentGridPosition.Status;
                ViewModel.Index = currentGridPosition.Index;

                Stories.StoriesMatrix[currentGridPosition.Status][currentGridPosition.Index] = ViewModel;

                LastGridPosition = currentGridPosition;

                //DrawTable(); // for debug only, no need to spam the console
            }

            return true;
        }

        protected override bool DoMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Canvas canvas = VisualTreeHelperExtensions.GetParent<Canvas>(AssociatedObject);

            if (!ViewModel.IsReadOnly)
                return false;

            int actualLeft = InitialStoryLeft + Convert.ToInt32(e.GetPosition(canvas).X - InitialLeftPos);
            int actualTop = InitialStoryTop + Convert.ToInt32(e.GetPosition(canvas).Y - InitialTopPos);

            Position newGridPosition = Stories.GetPosition(Convert.ToInt32(e.GetPosition(canvas).Y), Convert.ToInt32(e.GetPosition(canvas).X));

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                Stories.StoriesMatrix[newGridPosition.Status][newGridPosition.Index] != null &&
                !object.ReferenceEquals(Stories.StoriesMatrix[newGridPosition.Status][newGridPosition.Index], ViewModel))
            {
                // We'll merge the moving story with the one which is in the target location
                UserStoryStackViewModel stack = new UserStoryStackViewModel(Stories);
                foreach (var item in new List<IDraggableItem>() { Stories.StoriesMatrix[newGridPosition.Status][newGridPosition.Index], ViewModel })
                {
                    if (item is UserStoryViewModel)
                        stack.StackedUserStories.Add(item as UserStoryViewModel);
                    else
                        stack.StackedUserStories.AddRange<UserStoryViewModel>((item as UserStoryStackViewModel).StackedUserStories);

                    Stories.Stories.Remove(item);
                }

                Stories.AddExistingStory(stack, newGridPosition.Status, newGridPosition.Index);

            }
            else if (newGridPosition.IsValidPosition && object.ReferenceEquals(Stories.StoriesMatrix[newGridPosition.Status][newGridPosition.Index], ViewModel))
            {
                // Case where the story could be moved, we could make some space on the board
                // Everything has already been done
                // Now we'll just place the item correctly
                Stories.MoveObject(ViewModel, actualTop, newGridPosition.Top, actualLeft, newGridPosition.Left);
            }
            else
            {
                // Case where the move was refused : no space was available

                if (object.ReferenceEquals(Stories.StoriesMatrix[ViewModel.Status][ViewModel.Index], ViewModel))
                    Stories.StoriesMatrix[ViewModel.Status][ViewModel.Index] = null;

                ViewModel.Status = InitialGridPosition.Status;
                ViewModel.Index = InitialGridPosition.Index;

                Stories.MoveObject(ViewModel, actualTop, InitialStoryTop, actualLeft, InitialStoryLeft);
                Stories.StoriesMatrix[ViewModel.Status][ViewModel.Index] = ViewModel;
            }

            //DrawTable(); // for debug only, no need to spam the console

            return true;
        }

        protected override bool DoMouseRightButtonDown(MouseButtonEventArgs e)
        {
            ViewModel.IsReadOnly = true;
            ViewModel.QuickActionsVisible = false;

            // Avoid any other control to react to the click
            e.Handled = true;

            return false;
        }

        #endregion

        private void DrawTable()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Stories.BoardLayout.RowsCount; i++)
            {
                foreach (var column in Stories.BoardLayout.Columns)
                {
                    for (int j = 0; j < column.Width; j++)
                    {
                        if (Stories.StoriesMatrix[column.Status][i * column.Width + j] != null)
                            builder.Append('X');
                        else
                            builder.Append(' ');
                        builder.Append('|');
                    }
                }
                builder.AppendLine();
            }

            Console.WriteLine(builder.ToString());
        }
    }
}
