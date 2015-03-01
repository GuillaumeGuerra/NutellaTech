using Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OneNote.Service
{
    public class NotificationServiceProxy : INotificationService, IDisposable
    {
        private INotificationService _channel;

        public INotificationService Channel
        {
            get
            {
                if (_channel == null)
                    _channel = new DuplexChannelFactory<INotificationService>(new NotificationCallbackService(), "NotificationService").CreateChannel();

                return _channel;
            }
        }

        public IEnumerable<Notification> GetNotificationsForTopic(Topic topic)
        {
            return Channel.GetNotificationsForTopic(topic);
        }

        public List<Topic> GetAllTopics()
        {
            return Channel.GetAllTopics();
        }

        public void PushNotification(Notification notification, Topic topic)
        {
            Channel.PushNotification(notification, topic);
        }

        public void RegisterListener()
        {
            Channel.RegisterListener();
        }

        public void UnregisterListener()
        {
            Channel.UnregisterListener();
        }

        public void CreateTopic(Topic topic)
        {
            Channel.CreateTopic(topic);
        }

        public void VoteOnNotification(VoteNotification voteNotification, bool voteFor, string sender)
        {
            Channel.VoteOnNotification(voteNotification, voteFor, sender);
        }

        public List<Voter> GetVoters()
        {
            return Channel.GetVoters();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~NotificationServiceProxy()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
                GC.SuppressFinalize(this);
            if (_channel != null && _channel is IDisposable)
                (_channel as IDisposable).Dispose();
        }
    }
}
