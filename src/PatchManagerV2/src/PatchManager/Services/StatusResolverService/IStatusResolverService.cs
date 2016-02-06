using PatchManager.Services.ModelService;

namespace PatchManager.Services.StatusResolverService
{
    public interface IStatusResolverService
    {
        /// <summary>
        /// Fetch the last updates on gerrit and jira for a given gerrit
        /// </summary>
        /// <param name="gerrit"></param>
        void Resolve(GerritWithMetadata gerrit);

        /// <summary>
        /// In case the last update for gerrit and jira information were refreshed long enough ago, a resolution will happen
        /// </summary>
        /// <param name="gerrit"></param>
        void ResolveIfOutdated(GerritWithMetadata gerrit);
    }
}
