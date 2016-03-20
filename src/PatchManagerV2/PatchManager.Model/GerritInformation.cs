using PatchManager.Models;

namespace PatchManager.Services.Gerrit
{
    public class GerritInformation
    {
        public string JiraId { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public MergeStatus Status { get; set; }
    }
}