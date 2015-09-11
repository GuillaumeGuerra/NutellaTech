using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OneDbgClient.Framework;
using OneDbgClient.ViewModels;

namespace OneDbgClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ProcessesViewModel>();
            SimpleIoc.Default.Register<OneDbgMainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<IPopupService, PopupService>();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ServiceLocator.Current.GetInstance<IPopupService>().ShowError("Unhandled Exception", "UnhandledException caught !!!", e.ExceptionObject as Exception);
        }
    }

    public static class AppExtensions
    {
        public static ViewModelLocator ViewModelLocator(this Application app)
        {
            return app.FindResource("Locator") as ViewModelLocator;
        }
    }
}
