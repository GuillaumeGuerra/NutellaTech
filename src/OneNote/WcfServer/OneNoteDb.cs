using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WcfServer.DatabaseContext;

namespace WcfServer
{
    public class OneNoteDb : IDisposable
    {
        private static readonly OneNoteDb _instance = new OneNoteDb();
        public static OneNoteDb Instance
        {
            get
            {
                return _instance;
            }
        }

        public Dictionary<long, DbTopic> DbTopicsById { get; set; }
        public Dictionary<DbTopic, LinkedList<DbNotification>> DbTopicsIndex { get; set; }
        public Dictionary<long, Tuple<DbTopic, DbNotification>> DbNotificationsIndex { get; set; }

        public List<Topic> AllTopics { get; set; }
        public List<Voter> AllVoters { get; set; }

        private OneNoteContext DbContext { get; set; }

        private OneNoteDb()
        {
            DbContext = new OneNoteContext();
        }

        public void LoadContext()
        {
            DbContext.AllDbTopics.Load();
            DbContext.AllDbVoters.Load();
            DbContext.AllDbNotifications.Load();

            DbTopicsById = DbContext.AllDbTopics.Local.ToDictionary(t => t.Id);
            DbTopicsIndex = DbContext.AllDbTopics.Local.ToDictionary(t => t, t => new LinkedList<DbNotification>(t.Notifications));
            AllTopics = DbContext.AllDbTopics.Local.Select(t => t.ToTopic()).ToList();

            DbNotificationsIndex = new Dictionary<long, Tuple<DbTopic, DbNotification>>();

            foreach (var topic in DbContext.AllDbTopics)
            {
                foreach (var notif in topic.Notifications)
                {
                    DbNotificationsIndex.Add(notif.Id, new Tuple<DbTopic, DbNotification>(topic, notif));
                }
            }

            AllVoters = DbContext.AllDbVoters.Local.Select(v => v.ToVoter()).ToList();
        }

        public void IndexNotification(DbNotification notification, DbTopic topic)
        {
            DbTopicsIndex[topic].AddFirst(notification);
            DbNotificationsIndex.Add(notification.Id, new Tuple<DbTopic, DbNotification>(topic, notification));
        }

        public long AddTopic(Topic topic)
        {
            var dbTopic = DbTopic.FromTopic(topic);
            dbTopic = DbContext.AllDbTopics.Add(dbTopic);
            DbContext.SaveChanges();

            dbTopic.Notifications=new List<DbNotification>();
            DbTopicsIndex.Add(dbTopic, new LinkedList<DbNotification>());
            DbTopicsById.Add(dbTopic.Id, dbTopic);

            return dbTopic.Id;
        }

        public long AddNotification(Notification notification, Topic topic)
        {
            var dbNotif = DbNotification.FromNotification(notification);
            DbTopic dbTopic = DbTopicsById[topic.Id];
            dbNotif.Topic = dbTopic;
            DbContext.AllDbNotifications.Add(dbNotif);
            DbContext.SaveChanges();

            IndexNotification(dbNotif, dbTopic);

            return dbNotif.Id;
        }

        public void AddVoteToNotification(DbTopic topic, DbVoteNotification notification, Voter voter)
        {
            notification.Voters.Add(new DbVoter() { Name = voter.Name, VoteFor = voter.VoteFor });

            DbTopicsIndex[topic].Remove(notification);
            DbTopicsIndex[topic].AddFirst(notification);

            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
