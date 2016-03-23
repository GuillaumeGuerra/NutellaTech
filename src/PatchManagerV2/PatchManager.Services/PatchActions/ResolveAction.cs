using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("resolve")]
    public class ResolveAction : IPatchAction
    {
        public IJiraService Service { get; set; }

        public ResolveAction(IJiraService service)
        {
            Service = service;
        }

        public bool Apply(Patch patch)
        {
            if (patch.Status.Jira == JiraStatus.Resolved || patch.Status.Jira == JiraStatus.Closed)
                return false;

            if (Service.Resolve(patch.Jira.Id))
                patch.Status.Jira = JiraStatus.Resolved;
            else
            {
                // We don't know what happened here, so we'll move the status back to Unknown
                patch.Status.Jira = JiraStatus.Unknown;
            }

            return true;
        }
    }
}