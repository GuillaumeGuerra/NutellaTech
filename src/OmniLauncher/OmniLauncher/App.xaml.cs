using System;
using System.Windows;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using OmniLauncher.Services.IExceptionManager;
using OmniLauncher.Services.LauncherService;
using OmniLauncher.Services.RadialMenuItemBuilder;
using OmniLauncher.ViewModels;

namespace OmniLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureDependencyInjection();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void ConfigureDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<OmniLauncherViewModel>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<LauncherService>().As<ILauncherService>().PropertiesAutowired();
            builder.RegisterType<RadialMenuItemBuilder>().As<IRadialMenuItemBuilder>().PropertiesAutowired();

            Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ServiceLocator.Current.GetInstance<IMessageService>().ShowException(e.ExceptionObject as Exception);
        }
    }
}
