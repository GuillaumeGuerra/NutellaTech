using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer.DatabaseContext
{
    [Table("Notifications")]
    public class DbVoteNotification : DbNotification
    {
        public virtual ICollection<DbVoter> Voters { get; set; }

        public DbVoteNotification(DbNotification from)
            : base(from)
        {
            Voters = new List<DbVoter>();
        }

        public DbVoteNotification() { }
    }
}
