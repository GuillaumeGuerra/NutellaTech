using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public interface IGerritAction
    {
        bool Apply(Patch patch);
    }
}