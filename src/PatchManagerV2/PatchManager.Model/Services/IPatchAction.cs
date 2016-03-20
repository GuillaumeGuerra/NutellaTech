using PatchManager.Models;

namespace PatchManager.Model.Services
{
    public interface IPatchAction
    {
        bool Apply(Patch patch);
    }
}