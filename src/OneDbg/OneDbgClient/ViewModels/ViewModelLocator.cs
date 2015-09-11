using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace OneDbgClient.ViewModels
{
    public class ViewModelLocator
    {
        public ProcessesViewModel Processes
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProcessesViewModel>();
            }
        }

        public OneDbgMainViewModel OneDbgMain
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OneDbgMainViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }
    }
}