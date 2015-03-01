using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer.DatabaseContext
{
    [Table("Notifications")]
    public class DbNotification
    {
        public Int64 Id { get; set; }

        public string Text { get; set; }

        public string Sender { get; set; }

        public CriticityEnum Criticity { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        public virtual DbTopic Topic { get; set; }

        public DbNotification(DbNotification from)
        {
            Id = from.Id;
            Text = from.Text;
            Criticity = from.Criticity;
            NotificationType = from.NotificationType;
        }

        public DbNotification() { }

        internal Notification ToNotification()
        {
            Notification notif = null;
            if (this is DbVoteNotification)
                notif = new VoteNotification() { Voters = (this as DbVoteNotification).Voters.Select(v => v.ToVoter()).ToList() };
            else
                notif = new Notification();

            notif.Criticity = this.Criticity;
            notif.Id = this.Id;
            notif.NotificationType = this.NotificationType;
            notif.Sender = this.Sender;
            notif.Text = this.Text;

            return notif;
        }

        internal static DbNotification FromNotification(Notification notification)
        {
            DbNotification dbNotif = null;
            if (notification is VoteNotification)
                dbNotif = new DbVoteNotification();
            else
                dbNotif = new DbNotification();

            dbNotif.Criticity = notification.Criticity;
            dbNotif.Id = notification.Id;
            dbNotif.NotificationType = notification.NotificationType;
            dbNotif.Sender = notification.Sender;
            dbNotif.Text = notification.Text;

            return dbNotif;
        }
    }
}
