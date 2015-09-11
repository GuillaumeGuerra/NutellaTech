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

        public MainWindowViewModel MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
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