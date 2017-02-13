using System.Collections.ObjectModel;
using System.Windows;
using Autofac;
using Infragistics.Controls.Menus;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.RadialMenuItemBuilder;
using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.ViewModels
{
    public class OmniLauncherViewModel : DependencyObject
    {
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(ObservableCollection<RadialMenuItem>), typeof(OmniLauncherViewModel));

        public ObservableCollection<RadialMenuItem> Launchers
        {
            get { return (ObservableCollection<RadialMenuItem>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public IRadialMenuItemBuilder RadialMenuItemBuilder { get; set; }

        public OmniLauncherViewModel()
        {
            App.Container.InjectProperties(this);

            //Task.Run(() =>
            //{
            //    Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            var xmlConfiguration = new XmlLauncherConfigurationReader().LoadFile("Configuration/Launchers.xml");
            Launchers = RadialMenuItemBuilder.BuildMenuItems(new LauncherConfigurationProcessor().ProcessConfiguration(xmlConfiguration));
            //});
        }
    }
}
