namespace PatchManager.Services.JiraService
{
    public interface IJiraService
    {
        JiraInformation GetJiraInformation(string jiraId);
        bool Resolve(string jiraId);
    }
}
