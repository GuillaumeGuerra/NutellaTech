﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class VoteNotification : Notification
    {
        [DataMember]
        public List<Voter> Voters { get; set; }

        public VoteNotification(Notification from)
            : base(from)
        {
            Voters = new List<Voter>();
        }

        public VoteNotification() { }
    }
}
