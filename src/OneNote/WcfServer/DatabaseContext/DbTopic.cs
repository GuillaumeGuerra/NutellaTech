using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer.DatabaseContext
{
    [Table("Topics")]
    public class DbTopic
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public string Creator { get; set; }

        public virtual ICollection<DbNotification> Notifications { get; set; }

        public override int GetHashCode()
        {
            return (Name ?? string.Empty).GetHashCode() ^ (Creator ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherTopic = obj as DbTopic;
            return otherTopic != null && otherTopic.Name == this.Name && otherTopic.Creator == this.Creator;
        }

        internal static DbTopic FromTopic(Model.Topic topic)
        {
            return new DbTopic()
            {
                Creator = topic.Creator,
                Id = topic.Id,
                Name = topic.Name
            };
        }

        internal Model.Topic ToTopic()
        {
            return new Model.Topic()
            {
                Creator = this.Creator,
                Id = this.Id,
                Name = this.Name
            };
        }
    }
}
