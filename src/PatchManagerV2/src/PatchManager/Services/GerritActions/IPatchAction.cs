using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public interface IPatchAction
    {
        bool Apply(Patch patch);
    }
}