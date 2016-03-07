using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("ask")]
    public class AskedRegistrationAction : RegistrationStatusAction
    {
        public AskedRegistrationAction() : base(RegistrationStatus.Asked) { }
    }
}