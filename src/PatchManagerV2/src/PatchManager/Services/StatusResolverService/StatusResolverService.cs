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

        public void Resolve(GerritWithMetadata gerrit)
        {
            if (gerrit.Patch == null)
                return;
            if (gerrit.Patch.Status == null)
                gerrit.Patch.Status = new GerritStatus();

            ResolveGerritIfNecessary(gerrit);
            ResolveJiraIfNecessary(gerrit);

            gerrit.LastRefresh = DateTime.Now;
        }

        private void ResolveJiraIfNecessary(GerritWithMetadata gerrit)
        {
            if (gerrit.Patch.Status.Jira == JiraStatus.Closed)
            {
                // In that case, the jira cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var jiraMetadata = Jira.GetJiraInformation(gerrit.Patch.Jira.Id);
            gerrit.Patch.Jira.Description = jiraMetadata.Description;
            gerrit.Patch.Status.Jira = jiraMetadata.Status;
        }

        private void ResolveGerritIfNecessary(GerritWithMetadata gerrit)
        {
            if (gerrit.Patch.Status.Merge == MergeStatus.Merged)
            {
                // In that case, the gerrit cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var gerritMetadata = Gerrit.GetGerritInformation(gerrit.Patch.Id);

            gerrit.Patch.Author = gerritMetadata.Owner;
            gerrit.Patch.Title = gerritMetadata.Title;
            gerrit.Patch.Status.Merge = gerritMetadata.Status;

            if (gerrit.Patch.Jira == null || gerrit.Patch.Jira.Id != gerritMetadata.JiraId)
            {
                gerrit.Patch.Jira = new Jira()
                {
                    Id = gerritMetadata.JiraId
                };
                
                // Don't forget to update the jira status, to force a call to jira API
                // If the jira status was already merged, we wouldn't refresh it, so as we know the associated jira has changed, let's be defensive and force a refresh
                gerrit.Patch.Status.Jira = JiraStatus.Unknown;
            }
        }

        public void ResolveIfOutdated(GerritWithMetadata gerrit)
        {
            if (gerrit.LastRefresh < DateTime.Now.AddMinutes(-Startup.Settings.TimeoutInMinutesToResolveGerritStatus))
                Resolve(gerrit);
        }
    }
}