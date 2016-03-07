using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    public interface IPatchAction
    {
        bool Apply(Patch patch);
    }
}