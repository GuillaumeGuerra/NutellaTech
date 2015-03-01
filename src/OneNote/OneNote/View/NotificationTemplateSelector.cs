using Model;
using OneNote.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OneNote.View
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MessageDataTemplate { get; set; }
        public DataTemplate VoteDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var notif = item as NotificationViewModel;

            switch (notif.Notification.NotificationType)
            {
                case NotificationTypeEnum.Message:
                    return MessageDataTemplate;
                case NotificationTypeEnum.Vote:
                    return VoteDataTemplate;
            }

            return null;
        }
    }
}
