using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("refuse")]
    public class RefuseRegistrationAction : RegistrationStatusAction
    {
        public RefuseRegistrationAction() : base(RegistrationStatus.Refused) { }
    }
}