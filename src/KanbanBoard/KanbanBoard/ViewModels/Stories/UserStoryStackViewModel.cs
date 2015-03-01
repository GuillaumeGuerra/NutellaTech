using Framework;
using Framework.Extensions;
using GalaSoft.MvvmLight.Command;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KanbanBoard.ViewModels
{
    public class UserStoryStackViewModel : DraggableItemViewModel
    {
        #region Properties

        public static readonly DependencyProperty StackedUserStoriesProperty =
            DependencyProperty.Register("StackedUserStories", typeof(ObservableCollection<UserStoryViewModel>), typeof(UserStoryStackViewModel));
        public static readonly DependencyProperty StackButtonOpacityProperty =
            DependencyProperty.Register("StackButtonOpacity", typeof(double), typeof(UserStoryStackViewModel), new PropertyMetadata(0D));

        public double StackButtonOpacity
        {
            get { return (double)GetValue(StackButtonOpacityProperty); }
            set { SetValue(StackButtonOpacityProperty, value); }
        }

        public ObservableCollection<UserStoryViewModel> StackedUserStories
        {
            get { return (ObservableCollection<UserStoryViewModel>)GetValue(StackedUserStoriesProperty); }
            set { SetValue(StackedUserStoriesProperty, value); }
        }

        public ICommand SwitchToPreviousUserStoryCommand
        {
            get { return new RelayCommand(SwitchToPreviousUserStory); }
        }

        public ICommand SwitchToNextUserStoryCommand
        {
            get { return new RelayCommand(SwitchToNextUserStory); }
        }

        public ICommand UnstackStoryCommand
        {
            get { return new RelayCommand(UnstackStory); }
        }

        public override string Status
        {
            get { return StackedUserStories[0].Status; }
            set { StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.Status = value); }
        }

        public override int Index
        {
            get { return StackedUserStories[0].Index; }
            set { StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.Index = value); }
        }

        #endregion

        public UserStoryStackViewModel(UserStoriesViewModel allStories)
            : base(allStories)
        {
            StackedUserStories = new ObservableCollection<UserStoryViewModel>();
        }

        public void SwitchToNextUserStory()
        {
            UserStoryViewModel lastUS = StackedUserStories.Last<UserStoryViewModel>();
            StackedUserStories.RemoveAt(StackedUserStories.Count - 1);
            StackedUserStories.Insert(0, lastUS);
        }

        public void SwitchToPreviousUserStory()
        {
            UserStoryViewModel firstUS = StackedUserStories[0];
            StackedUserStories.RemoveAt(0);
            StackedUserStories.Add(firstUS);
        }

        protected override void IsReadOnlyValueChanged(DependencyPropertyChangedEventArgs e)
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.IsReadOnly = (bool)e.NewValue);
        }

        public override void DropToolboxItem(IToolboxItem item)
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.DropToolboxItem(item));
        }

        public override void DeleteAvatar()
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.DeleteAvatar());
        }

        private void UnstackStory()
        {
            UserStoryViewModel story = StackedUserStories[StackedUserStories.Count - 1];
            List<StoryMove> moves = AllStories.IdentifyBlockingStories(story.Status, story.Index + 1);

            if (moves != null)
            {
                AllStories.MoveStories(moves);

                StackedUserStories.Remove(story);
                AllStories.AddExistingStory(story, story.Status, story.Index + 1);

                if (StackedUserStories.Count == 1)
                    TurnStackIntoStory();
            }
        }

        private void TurnStackIntoStory()
        {
            AllStories.AddExistingStory(StackedUserStories[0], Status, Index);
            AllStories.Stories.Remove(this);
        }

        public override void AssignTodayToStartDate()
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.AssignTodayToStartDate());
        }

        public override void AssignTodayToDevDoneDate()
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.AssignTodayToDevDoneDate());
        }

        public override void AssignTodayToEndDate()
        {
            StackedUserStories.ToList<UserStoryViewModel>().ForEach(s => s.AssignTodayToEndDate());
        }
    }
}
