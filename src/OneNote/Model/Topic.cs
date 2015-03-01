using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Topic
    {
        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Creator { get; set; }

        //[IgnoreDataMember]
        //public virtual ICollection<Notification> Notifications { get; set; }

        public override int GetHashCode()
        {
            return (Name ?? string.Empty).GetHashCode() ^ (Creator ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherTopic = obj as Topic;
            return otherTopic != null && otherTopic.Name == this.Name && otherTopic.Creator == this.Creator;
        }
    }
}
