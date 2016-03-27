using System;
using PatchManager.Config;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.Context;
using PatchManager.Services.Model;

namespace PatchManager.Services.StatusResolver
{
    public class StatusResolverService : IStatusResolverService
    {
        public IPatchManagerContextService Context { get; set; }
        public IGerritService Gerrit { get; set; }
        public IJiraService Jira { get; set; }

        public StatusResolverService(IPatchManagerContextService context, IGerritService gerrit, IJiraService jira)
        {
            Context = context;
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

            patch.LastRefresh = Context.Now;
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
            if (patch.Patch.Status.Gerrit == GerritStatus.Merged)
            {
                // In that case, the gerrit cannot be updated any more, so there's no reason to fetch new information
                return;
            }

            var gerritMetadata = Gerrit.GetGerritInformation(patch.Patch.Gerrit.Id);

            patch.Patch.Gerrit.Author = gerritMetadata.Owner;
            patch.Patch.Gerrit.Description = gerritMetadata.Title;
            patch.Patch.Status.Gerrit = gerritMetadata.Status;

            if (patch.Patch.Jira == null || patch.Patch.Jira.Id != gerritMetadata.JiraId)
            {
                patch.Patch.Jira = new Models.Jira()
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
            if (patch.LastRefresh < Context.Now.AddMinutes(-Context.Settings.TimeoutInMinutesToResolveGerritStatus))
                Resolve(patch);
        }
    }
}