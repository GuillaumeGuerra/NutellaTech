using GalaSoft.MvvmLight;
using Model;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneNote.ViewModel
{
    public class NotificationsViewModel : ViewModelBase
    {
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<NotificationViewModel>();
        }

        private ObservableCollection<NotificationViewModel> _notifications;
        public ObservableCollection<NotificationViewModel> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                RaisePropertyChanged("Notifications");
            }
        }

        public void RemoveNotification(NotificationViewModel notification)
        {
            Notifications.Remove(notification);
        }

        public void InitializeCollection(IEnumerable<NotificationViewModel> notifs)
        {
            Notifications = new ObservableCollection<NotificationViewModel>(notifs);

            foreach (var notif in Notifications)
            {
                PrepareNotification(notif);
            }
        }

        public void PushNotification(NotificationViewModel notif)
        {
            PrepareNotification(notif);
            Notifications.Insert(0, notif);
        }

        public void UpdateNotification(NotificationViewModel notif)
        {
            for (int i = 0; i < Notifications.Count; i++)
            {
                if (Notifications[i].Notification.Id == notif.Notification.Id)
                {
                    Notifications.RemoveAt(i);
                    break;
                }
            }

            PushNotification(notif);
        }

        private void PrepareNotification(NotificationViewModel notif)
        {
            notif.OnNotificationRemoved += RemoveNotification;
        }
    }
}
