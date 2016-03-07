using System;
using PatchManager.Models;
using PatchManager.Services.GerritService;
using PatchManager.Services.JiraService;
using PatchManager.Services.ModelService;

namespace PatchManager.Services.StatusResolverService
{
    public class StatusResolverService : IStatusResolverService
    {
        public IGerritService Gerrit { get; set; }
        public IJiraService Jira { get; set; }

        public StatusResolverService(IGerritService gerrit, IJiraService jira)
        {
            Gerrit = gerrit;
            Jira = jira;
        }

        public void Resolve(PatchWithMetadata patch)
        {
            if (patch.Patch == null)
                return;

            if (patch.Patch.Status == null)
                patch.Patch.Status = new PatchStatus();

            ResolveGerritIfNecessary(patch);
            ResolveJiraIfNecessary(patch);

            patch.LastRefresh = DateTime.Now;
        }

        private void ResolveJiraIfNecessary(PatchWithMetadata patch)
        {
            if (patch.Patch.Status.Jira == JiraStatus.Closed)
            {
                // In that case, the jira cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var jiraMetadata = Jira.GetJiraInformation(patch.Patch.Jira.Id);
            patch.Patch.Jira.Description = jiraMetadata.Description;
            patch.Patch.Status.Jira = jiraMetadata.Status;
        }

        private void ResolveGerritIfNecessary(PatchWithMetadata patch)
        {
            if (patch.Patch.Status.Merge == MergeStatus.Merged)
            {
                // In that case, the gerrit cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var gerritMetadata = Gerrit.GetGerritInformation(patch.Patch.Gerrit.Id);

            patch.Patch.Gerrit.Author = gerritMetadata.Owner;
            patch.Patch.Gerrit.Description = gerritMetadata.Title;
            patch.Patch.Status.Merge = gerritMetadata.Status;

            if (patch.Patch.Jira == null || patch.Patch.Jira.Id != gerritMetadata.JiraId)
            {
                patch.Patch.Jira = new Jira()
                {
                    Id = gerritMetadata.JiraId
                };
                
                // Don't forget to update the jira status, to force a call to jira API
                // If the jira status was already merged, we wouldn't refresh it, so as we know the associated jira has changed, let's be careful and force a refresh
                patch.Patch.Status.Jira = JiraStatus.Unknown;
            }
        }

        public void ResolveIfOutdated(PatchWithMetadata patch)
        {
            if (patch.LastRefresh < DateTime.Now.AddMinutes(-Startup.Settings.TimeoutInMinutesToResolveGerritStatus))
                Resolve(patch);
        }
    }
}