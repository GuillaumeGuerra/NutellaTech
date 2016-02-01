using PatchManager.Models;

namespace PatchManager.Services.GerritService
{
    public class GerritMetadata
    {
        public string JiraId { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public MergeStatus Status { get; set; }
    }
}