using Microsoft.Practices.ServiceLocation;

namespace OmniLauncher.ViewModels
{
    public class ViewModelLocator
    {
        public OmniLauncherViewModel MainViewModel => ServiceLocator.Current.GetInstance<OmniLauncherViewModel>();
    }
}