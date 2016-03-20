using PatchManager.Services.Model;

namespace PatchManager.Model.Services
{
    public interface IStatusResolverService
    {
        /// <summary>
        /// Fetch the last updates on gerrit and jira for a given patch
        /// </summary>
        /// <param name="patch"></param>
        void Resolve(PatchWithMetadata patch);

        /// <summary>
        /// In case the last update for gerrit and jira information were refreshed long enough ago, a resolution will happen
        /// </summary>
        /// <param name="patch"></param>
        void ResolveIfOutdated(PatchWithMetadata patch);
    }
}
