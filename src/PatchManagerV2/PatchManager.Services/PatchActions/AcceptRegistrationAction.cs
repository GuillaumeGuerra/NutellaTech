using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("accept")]
    public class AcceptRegistrationAction : RegistrationStatusAction
    {
        public AcceptRegistrationAction() : base(RegistrationStatus.Accepted) { }
    }
}