using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KanbanBoard.Entities
{
    public class UserStory : DependencyObject
    {
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty IWantToProperty =
            DependencyProperty.Register("IWantTo", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty InOrderToProperty =
            DependencyProperty.Register("InOrderTo", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty EpicProperty =
            DependencyProperty.Register("Epic", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty UsIdProperty =
            DependencyProperty.Register("UsId", typeof(string), typeof(UserStory));
        public static readonly DependencyProperty IsStopDevProperty =
            DependencyProperty.Register("IsStopDev", typeof(bool), typeof(UserStory));
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime?), typeof(UserStory));
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime?), typeof(UserStory));
        public static readonly DependencyProperty DevDoneDateProperty =
            DependencyProperty.Register("DevDoneDate", typeof(DateTime?), typeof(UserStory));
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(Avatar), typeof(UserStory));
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(UserStory), new PropertyMetadata(0));
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(UserStory));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        
        public string UsId
        {
            get { return (string)GetValue(UsIdProperty); }
            set { SetValue(UsIdProperty, value); }
        }

        public string IWantTo
        {
            get { return (string)GetValue(IWantToProperty); }
            set { SetValue(IWantToProperty, value); }
        }

        public string Project
        {
            get { return (string)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public string InOrderTo
        {
            get { return (string)GetValue(InOrderToProperty); }
            set { SetValue(InOrderToProperty, value); }
        }

        public string Epic
        {
            get { return (string)GetValue(EpicProperty); }
            set { SetValue(EpicProperty, value); }
        }

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public bool IsStopDev
        {
            get { return (bool)GetValue(IsStopDevProperty); }
            set { SetValue(IsStopDevProperty, value); }
        }

        public DateTime? StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public DateTime? EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public DateTime? DevDoneDate
        {
            get { return (DateTime?)GetValue(DevDoneDateProperty); }
            set { SetValue(DevDoneDateProperty, value); }
        }

        public Avatar Avatar
        {
            get { return (Avatar)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }
    }
}
