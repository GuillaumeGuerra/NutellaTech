/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:OmniLauncher"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace OmniLauncher.ViewModel
{
    public class ViewModelLocator
    {
        //public ProcessesViewModel Processes
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<ProcessesViewModel>();
        //    }
        //}

        //public OneDbgMainViewModel OneDbgMain
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<OneDbgMainViewModel>();
        //    }
        //}

        //public SettingsViewModel Settings
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<SettingsViewModel>();
        //    }
        //}
    }
}