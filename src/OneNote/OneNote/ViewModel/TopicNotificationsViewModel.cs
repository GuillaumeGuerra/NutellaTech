using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class TopicNotificationsViewModel : ViewModelBase
    {
        public TopicNotificationsViewModel()
        {
            if (IsInDesignMode)
            {
                Topic = new TopicViewModel() { Topic = new Topic() { Name = "My ass is too big ...", Creator = "J-Lo" } };
                Notifications = new NotificationsViewModel()
                {
                    Notifications =
                        new ObservableCollection<NotificationViewModel>(){
                    new NotificationViewModel(){Notification= new Notification(){Text="My ass rocks !", Sender="Yoda !",Criticity=CriticityEnum.Low}},
                    new NotificationViewModel(){Notification= new Notification(){Text="Yoda's one too !", Sender="Luke !",Criticity=CriticityEnum.High,NotificationType=NotificationTypeEnum.Vote}}
                    }
                };
            }

            NewNotification = new NotificationViewModel();
        }

        private TopicViewModel _topic;
        public TopicViewModel Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                RaisePropertyChanged("Topic");
            }
        }

        private NotificationsViewModel _notifications;
        public NotificationsViewModel Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                RaisePropertyChanged("Notifications");
            }
        }

        private NotificationViewModel _newNotification;
        public NotificationViewModel NewNotification
        {
            get { return _newNotification; }
            set
            {
                _newNotification = value;
                RaisePropertyChanged("NewNotification");
            }
        }

        public ICommand SendNotificationCommand
        {
            get
            {
                return new RelayCommand(SendNotification);
            }
        }

        private void SendNotification()
        {
            using (var proxy = new NotificationServiceProxy())
            {
                NewNotification.Notification.Sender = App.Current.GetLocator().Settings.UserName;
                proxy.PushNotification(NewNotification.Notification, Topic.Topic);
            }
        }

        public void LoadExistingNotifications()
        {
            using (var proxy = new NotificationServiceProxy())
            {
                Notifications = new NotificationsViewModel();
                Notifications.InitializeCollection(proxy.GetNotificationsForTopic(Topic.Topic).Select(n => NotificationViewModelFactory.Get(n)));
            }
        }
    }
}
