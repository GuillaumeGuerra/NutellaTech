using System;
using System.Collections.Generic;
using PatchManager.Controllers;

namespace PatchManager.Models
{
    public class Patch
    {
        public string Version { get; set; }
        public string ReleaseManager { get; set; }
        public string ReleaseManagerMail { get; set; }
        public bool IsCurrent { get; set; }
        public IEnumerable<Gerrit> Gerrits { get; set; }
        public DateTime Date { get; set; }
    }
}