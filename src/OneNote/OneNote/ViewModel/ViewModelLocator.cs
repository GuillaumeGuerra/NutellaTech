/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:OneNote"
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
using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneNote.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<NotificationsViewModel>();
            SimpleIoc.Default.Register<AllTopicsViewModel>();
            SimpleIoc.Default.Register<NewTopicViewModel>();
            SimpleIoc.Default.Register<AllTopicsViewModel>();
            SimpleIoc.Default.Register<ChosenTopicsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();            
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public NotificationsViewModel Notifications
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NotificationsViewModel>();
            }
        }

        public NewTopicViewModel NewTopic
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewTopicViewModel>();
            }
        }

        public List<CriticityEnum> AllCriticities
        {
            get
            {
                return new List<CriticityEnum>() { CriticityEnum.Low, CriticityEnum.Medium, CriticityEnum.High };
            }
        }

        public List<NotificationTypeEnum> AllNotificationTypes
        {
            get
            {
                return new List<NotificationTypeEnum>() { NotificationTypeEnum.Message, NotificationTypeEnum.Vote };
            }
        }

        public AllTopicsViewModel AllTopics
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AllTopicsViewModel>();
            }
        }

        public ChosenTopicsViewModel ChosenTopics
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChosenTopicsViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}