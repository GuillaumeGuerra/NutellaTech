using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract(IsReference = true)]
    [Serializable]
    [KnownType(typeof(VoteNotification))]
    public class Notification
    {
        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        public CriticityEnum Criticity { get; set; }

        [DataMember]
        public NotificationTypeEnum NotificationType { get; set; }

        public Notification(Notification from)
        {
            Id = from.Id;
            Text = from.Text;
            Criticity = from.Criticity;
            NotificationType = from.NotificationType;
        }

        public Notification()
        {
            Criticity = CriticityEnum.Low;
            NotificationType = NotificationTypeEnum.Message;
        }
    }
}
