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
            if (gerrit.Gerrit == null)
                return;
            if (gerrit.Gerrit.Status == null)
                gerrit.Gerrit.Status = new GerritStatus();


            ResolveGerritIfNecessary(gerrit);
            ResolveJiraIfNecessary(gerrit);

            gerrit.LastRefresh = DateTime.Now;
        }

        private void ResolveJiraIfNecessary(GerritWithMetadata gerrit)
        {
            if (gerrit.Gerrit.Status.Jira == JiraStatus.Closed)
            {
                // In that case, the jira cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var jiraMetadata = Jira.GetJiraInformation(gerrit.Gerrit.Jira);
            gerrit.Gerrit.Status.Jira = jiraMetadata.Status;
        }

        private void ResolveGerritIfNecessary(GerritWithMetadata gerrit)
        {
            if (gerrit.Gerrit.Status.Merge == MergeStatus.Merged)
            {
                // In that case, the gerrit cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var gerritMetadata = Gerrit.GetGerritInformation(gerrit.Gerrit.Id);

            gerrit.Gerrit.Owner = gerritMetadata.Owner;
            gerrit.Gerrit.Jira = gerritMetadata.JiraId;
            gerrit.Gerrit.Title = gerritMetadata.Title;
            gerrit.Gerrit.Status.Merge = gerritMetadata.Status;
        }

        public void ResolveIfOutdated(GerritWithMetadata gerrit)
        {
            if (gerrit.LastRefresh < DateTime.Now.AddMinutes(-Startup.Settings.TimeoutInMinutesToResolveGerritStatus))
                Resolve(gerrit);
        }
    }
}