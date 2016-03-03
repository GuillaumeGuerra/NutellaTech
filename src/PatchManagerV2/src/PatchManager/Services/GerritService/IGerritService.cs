namespace PatchManager.Services.GerritService
{
    public interface IGerritService
    {
        GerritInformation GetGerritInformation(int gerritId);
        bool Merge(int gerritId);
    }
}
