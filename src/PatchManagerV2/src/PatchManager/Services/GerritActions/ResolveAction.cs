using PatchManager.Models;
using PatchManager.Services.JiraService;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("resolve")]
    public class ResolveAction : IGerritAction
    {
        public IJiraService Service { get; set; }

        public ResolveAction(IJiraService service)
        {
            Service = service;
        }

        public bool Apply(Gerrit gerrit)
        {
            if (gerrit.Status.Jira == JiraStatus.Resolved)
                return false;

            if (Service.Resolve(gerrit.Jira.Id))
                gerrit.Status.Jira = JiraStatus.Resolved;

            return true;
        }
    }
}