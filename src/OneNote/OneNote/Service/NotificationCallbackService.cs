using Interface;
using Microsoft.Practices.ServiceLocation;
using Model;
using OneNote.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneNote.Service
{
    public class NotificationCallbackService : INotificationCallbackService
    {
        public void NewNotificationReceived(Notification notif, Topic topic, UpdateType updateType)
        {
            var matchingTopic = App.Current.GetLocator().ChosenTopics.ChosenTopics.FirstOrDefault(t => t.Topic.Topic.Equals(topic));
            if (matchingTopic != null)
            {
                if (updateType == UpdateType.NewNotification)
                    matchingTopic.Notifications.PushNotification(NotificationViewModelFactory.Get(notif));
                else
                    matchingTopic.Notifications.UpdateNotification(NotificationViewModelFactory.Get(notif));
            }
        }

        public void NewTopicCreated(Topic topic)
        {
            App.Current.GetLocator().AllTopics.AllTopics.Add(new TopicViewModel() { Topic = topic });
        }
    }
}
