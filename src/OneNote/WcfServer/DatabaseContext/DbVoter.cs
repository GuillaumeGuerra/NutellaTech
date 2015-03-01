using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WcfServer.DatabaseContext
{
    [Table("Voters")] 
    public class DbVoter
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public bool VoteFor { get; set; }

        public virtual DbVoteNotification VoteNotification { get; set; }

        internal Voter ToVoter()
        {
            return new Voter()
            {
                Id = this.Id,
                Name = this.Name,
                VoteFor = this.VoteFor
            };
        }
    }
}
