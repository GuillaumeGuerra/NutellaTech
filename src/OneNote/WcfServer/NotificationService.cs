using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfServer.DatabaseContext;
using System.Data.Entity;

namespace WcfServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NotificationService : INotificationService
    {
        public List<INotificationCallbackService> Callbacks { get; set; }

        public NotificationService()
        {
            Callbacks = new List<INotificationCallbackService>();
        }

        public void RegisterListener()
        {
            Callbacks.Add(OperationContext.Current.GetCallbackChannel<INotificationCallbackService>());
        }

        public void UnregisterListener()
        {
            Callbacks.Remove(OperationContext.Current.GetCallbackChannel<INotificationCallbackService>());
        }

        public void PushNotification(Notification notification, Topic topic)
        {
            Console.WriteLine("Notification received from [{0}], criticity [{1}], type [{2}], topic [{3}], with text [{4}]", notification.Sender, notification.Criticity, notification.NotificationType, topic.Name, notification.Text);

            if (notification.NotificationType == NotificationTypeEnum.Vote)
                notification = new VoteNotification(notification) { Voters = new List<Voter>() { new Voter() { Name = notification.Sender, VoteFor = true } } };

            notification.Id = OneNoteDb.Instance.AddNotification(notification, topic);

            ForEachConsumers(c => c.NewNotificationReceived(notification, topic, UpdateType.NewNotification));
        }

        public IEnumerable<Notification> GetNotificationsForTopic(Topic topic)
        {
            return OneNoteDb.Instance.DbTopicsById[topic.Id].Notifications.Select(n => n.ToNotification());
        }

        public List<Topic> GetAllTopics()
        {
            return OneNoteDb.Instance.AllTopics;
        }

        public void CreateTopic(Topic topic)
        {
            topic.Id = OneNoteDb.Instance.AddTopic(topic);

            ForEachConsumers(c => c.NewTopicCreated(topic));
        }

        public void VoteOnNotification(VoteNotification voteNotification, bool voteFor, string sender)
        {
            Tuple<DbTopic, DbNotification> index = null;
            if (OneNoteDb.Instance.DbNotificationsIndex.TryGetValue(voteNotification.Id, out index))
            {
                var voteNotif = index.Item2 as DbVoteNotification;
                var dbTopic = index.Item1;

                if (voteNotif.Voters.Any(v => v.Name == sender))
                    return;

                OneNoteDb.Instance.AddVoteToNotification(dbTopic,voteNotif, new Voter() { Name = sender, VoteFor = voteFor });

                ForEachConsumers(c => c.NewNotificationReceived(voteNotif.ToNotification(), dbTopic.ToTopic(), UpdateType.Update));
            }
            else
            {
                // TODO : send again the notification to everyone ?
            }
        }

        public List<Voter> GetVoters()
        {
            return OneNoteDb.Instance.AllVoters;
        }

        private void ForEachConsumers(Action<INotificationCallbackService> action)
        {
            Callbacks.ForEach(c =>
            {
                try
                {
                    action(c);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error received while sending to consumer : " + e.ToString());
                }
            });
        }
    }
}
