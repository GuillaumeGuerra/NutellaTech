using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServer.DatabaseContext;

namespace WcfServer
{
    public class OneNoteContext : DbContext
    {
        public DbSet<DbNotification> AllDbNotifications { get; set; }
        public DbSet<DbVoteNotification> AllDbVoteNotifications { get; set; }
        public DbSet<DbTopic> AllDbTopics { get; set; }
        public DbSet<DbVoter> AllDbVoters { get; set; }
    }
}
