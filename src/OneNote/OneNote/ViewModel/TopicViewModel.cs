using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class TopicViewModel : ViewModelBase
    {
        public TopicViewModel()
        {
            Topic = new Topic();
            TopicStatus = TopicSubscribeStatus.Unsubscribed;
        }

        private Topic _topic;
        public Topic Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                RaisePropertyChanged("Name");
            }
        }

        private TopicSubscribeStatus _topicStatus;
        public TopicSubscribeStatus TopicStatus
        {
            get { return _topicStatus; }
            set
            {
                _topicStatus = value;
                RaisePropertyChanged("TopicStatus");

                SubscribeVisible = (value == TopicSubscribeStatus.Unsubscribed) ? Visibility.Visible : Visibility.Collapsed;
                UnsubscribeVisible = (value == TopicSubscribeStatus.Subscribed) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _unsubscibeVisible;
        public Visibility UnsubscribeVisible
        {
            get { return _unsubscibeVisible; }
            set
            {
                _unsubscibeVisible = value;
                RaisePropertyChanged("UnsubscribeVisible");
            }
        }

        private Visibility _subscibeVisible;
        public Visibility SubscribeVisible
        {
            get { return _subscibeVisible; }
            set
            {
                _subscibeVisible = value;
                RaisePropertyChanged("SubscribeVisible");
            }
        }

        public ICommand UnsubscribeCommand
        {
            get { return new RelayCommand(Unsubcribe); }
        }

        public ICommand SubscribeCommand
        {
            get { return new RelayCommand(Subscribe); }
        }

        private void Unsubcribe()
        {
            var topics = App.Current.GetLocator().ChosenTopics.ChosenTopics;
            topics.Remove(topics.First(t => t.Topic.Equals(this)));

            TopicStatus = TopicSubscribeStatus.Unsubscribed;
        }

        private void Subscribe()
        {
            var newTopic = new TopicNotificationsViewModel()
            {
                Topic = this,
                Notifications = new NotificationsViewModel()
            };
            App.Current.GetLocator().ChosenTopics.ChosenTopics.Add(newTopic);

            newTopic.LoadExistingNotifications();
            newTopic.Topic.TopicStatus = TopicSubscribeStatus.Subscribed;
        }
    }

    public enum TopicSubscribeStatus
    {
        Subscribed,
        Unsubscribed
    }
}
