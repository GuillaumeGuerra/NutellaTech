using System;

namespace PatchManager.Models
{
    public class Release
    {
        public string Version { get; set; }
        public string ReleaseManager { get; set; }
        public string ReleaseManagerMail { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime Date { get; set; }
    }
}