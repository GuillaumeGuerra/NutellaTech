using Interface;
using OneNote.Service;
using OneNote.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OneNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NotificationServiceProxy NotificationServiceProxy { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Delay Startup to let time for service to start
            Thread.Sleep(5000);

            //using (var proxy=new NotificationServiceProxy())
            //{
            //    var voters= proxy.GetVoters();
            //}

            base.OnStartup(e);

            NotificationServiceProxy = new NotificationServiceProxy();
            NotificationServiceProxy.RegisterListener();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            NotificationServiceProxy.UnregisterListener();
            NotificationServiceProxy.Dispose();
        }
    }

    public static class AppExtensions
    {
        public static ViewModelLocator GetLocator(this Application app)
        {
            return app.Resources["Locator"] as ViewModelLocator;
        }
    }
}
