using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("ask")]
    public class AskedRegistrationAction : RegistrationStatusAction
    {
        public AskedRegistrationAction() : base(RegistrationStatus.Asked) { }
    }
}