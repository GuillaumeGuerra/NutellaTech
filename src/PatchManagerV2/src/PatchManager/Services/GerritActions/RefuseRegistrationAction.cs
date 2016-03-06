using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("refuse")]
    public class RefuseRegistrationAction : RegistrationStatusAction
    {
        public RefuseRegistrationAction() : base(RegistrationStatus.Refused) { }
    }
}