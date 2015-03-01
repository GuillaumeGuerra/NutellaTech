using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneNote.ViewModel
{
    public static class NotificationViewModelFactory
    {
        public static NotificationViewModel Get(Notification notif)
        {
            if (notif is VoteNotification)
                return new VoteNotificationViewModel() { Notification = notif };
            else
                return new NotificationViewModel() { Notification = notif };
        }
    }
}
