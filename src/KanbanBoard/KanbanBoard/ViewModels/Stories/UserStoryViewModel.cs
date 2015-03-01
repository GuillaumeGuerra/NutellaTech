using Framework;
using GalaSoft.MvvmLight.Command;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KanbanBoard.ViewModels
{
    public class UserStoryViewModel : DraggableItemViewModel
    {
        public UserStoryViewModel() : base(null) { }

        public UserStoryViewModel(UserStoriesViewModel allStories, UserStory story)
            : base(allStories)
        {
            Story = story;
        }

        public static readonly DependencyProperty StoryProperty =
            DependencyProperty.Register("Story", typeof(UserStory), typeof(UserStoryViewModel));
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(AvatarViewModel), typeof(UserStoryViewModel));

        public ICommand SwitchReadOnlyModeCommand
        {
            get
            {
                return new RelayCommand(SwitchReadOnlyMode);
            }
        }

        public UserStory Story
        {
            get { return (UserStory)GetValue(StoryProperty); }
            set { SetValue(StoryProperty, value); }
        }

        public AvatarViewModel Avatar
        {
            get { return (AvatarViewModel)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        public override string Status
        {
            get { return Story.Status; }
            set { Story.Status = value; }
        }

        public override int Index
        {
            get { return Story.Index; }
            set { Story.Index = value; }
        }

        public override void DropToolboxItem(IToolboxItem item)
        {
            if (item is AvatarViewModel)
            {
                Story.Avatar = (item as AvatarViewModel).Avatar;
                Avatar = new AvatarViewModel() { Avatar = Story.Avatar };
            }
        }

        public override void DeleteAvatar()
        {
            Story.Avatar = null;
            Avatar = null;
        }

        public override void AssignTodayToStartDate()
        {
            Story.StartDate = DateTime.Today;
        }

        public override void AssignTodayToDevDoneDate()
        {
            Story.DevDoneDate = DateTime.Today;
        }

        public override void AssignTodayToEndDate()
        {
            Story.EndDate = DateTime.Today;
        }
        
        private void SwitchReadOnlyMode()
        {
            IsReadOnly = !IsReadOnly;
            QuickActionsVisible = !QuickActionsVisible;
        }
    }
}
