using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {
        private Notification _notification;
        public Notification Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                RaisePropertyChanged("Notification");
            }
        }

        public ICommand CloseCommand
        {
            get { return new RelayCommand(Close); }
        }

        public event Action<NotificationViewModel> OnNotificationRemoved;

        public NotificationViewModel()
        {
            Notification = new Notification();
        }

        private void Close()
        {
            if (OnNotificationRemoved != null)
                OnNotificationRemoved(this);
        }
    }
}
