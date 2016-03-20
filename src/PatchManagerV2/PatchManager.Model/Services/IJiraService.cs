using PatchManager.Services.Jira;

namespace PatchManager.Model.Services
{
    public interface IJiraService
    {
        JiraInformation GetJiraInformation(string jiraId);
        bool Resolve(string jiraId);
    }
}
