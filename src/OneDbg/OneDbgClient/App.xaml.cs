using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("UnhandledException caught : " + Environment.NewLine + e.ExceptionObject.ToString(),
                "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
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
