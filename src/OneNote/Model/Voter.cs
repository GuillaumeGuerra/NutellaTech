using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Model
{
    [DataContract]
    public class Voter
    {
        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool VoteFor { get; set; }

        //[DataMember]
        //public virtual VoteNotification VoteNotification { get; set; }
    }
}
