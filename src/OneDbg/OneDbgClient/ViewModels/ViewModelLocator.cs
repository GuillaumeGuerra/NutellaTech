using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace OneDbgClient.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ProcessesViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}