using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class NewNotificationViewModel : ViewModelBase
    {
        public NewNotificationViewModel()
        {
            Notification = new NotificationViewModel();
            Topic = new TopicViewModel();
        }

        private NotificationViewModel _notification;
        public NotificationViewModel Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                RaisePropertyChanged("Notification");
            }
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
    }
}
