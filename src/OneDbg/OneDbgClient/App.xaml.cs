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
    }

    public static class AppExtensions
    {
        public static ViewModelLocator ViewModelLocator(this Application app)
        {
            return app.FindResource("Locator") as ViewModelLocator;
        }
    }
}
