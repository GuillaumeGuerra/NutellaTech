using PatchManager.Models;

namespace PatchManager.Services.JiraService
{
    public class JiraInformation
    {
        public JiraStatus Status { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }
}