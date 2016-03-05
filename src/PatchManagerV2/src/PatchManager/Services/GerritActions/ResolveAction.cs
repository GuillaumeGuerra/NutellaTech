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

        public bool Apply(Patch patch)
        {
            if (patch.Status.Jira == JiraStatus.Resolved)
                return false;

            if (Service.Resolve(patch.Jira.Id))
                patch.Status.Jira = JiraStatus.Resolved;

            return true;
        }
    }
}