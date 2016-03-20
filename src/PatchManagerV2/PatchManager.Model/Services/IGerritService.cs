using PatchManager.Services.Gerrit;

namespace PatchManager.Model.Services
{
    public interface IGerritService
    {
        GerritInformation GetGerritInformation(int gerritId);
        bool Merge(int gerritId);
    }
}
