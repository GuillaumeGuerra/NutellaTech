using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;
using Autofac;
using Infragistics.Controls.Menus;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.LauncherService;

namespace OmniLauncher.Behaviors
{
    public class XamRadialMenuBehavior : Behavior<XamRadialMenu>
    {
        public static readonly DependencyProperty LaunchersProperty =
            DependencyProperty.Register("Launchers", typeof(LaunchersNode), typeof(XamRadialMenuBehavior), new PropertyMetadata(PropertyChangedCallback));

        public LaunchersNode Launchers
        {
            get { return (LaunchersNode)GetValue(LaunchersProperty); }
            set { SetValue(LaunchersProperty, value); }
        }

        public ILauncherService LauncherService { get; set; }

        public XamRadialMenuBehavior()
        {
            App.Container.InjectProperties(this);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((XamRadialMenuBehavior)dependencyObject).ApplyItems();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            ApplyItems();
        }

        private void ApplyItems()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.Items.Clear();
            foreach (var item in GetMenuItems(Launchers))
            {
                AssociatedObject.Items.Add(item);
            }
        }

        public List<RadialMenuItem> GetMenuItems(LaunchersNode launchers)
        {
            var items = new List<RadialMenuItem>();

            if (launchers != null)
            {
                foreach (var item in launchers.SubGroups)
                {
                    items.Add(GetNodeMenuItem(item));
                }
            }

            return items;
        }

        public RadialMenuItem GetNodeMenuItem(LaunchersNode launchers)
        {
            var item = new RadialMenuItem();
            item.Header = launchers.Header;

            foreach (var group in launchers.SubGroups)
            {
                item.Items.Add(GetNodeMenuItem(group));
            }
            foreach (var launcher in launchers.Launchers)
            {
                item.Items.Add(GetLauncherMenuItem(launcher));
            }

            return item;
        }

        private RadialMenuItem GetLauncherMenuItem(LauncherLink launcher)
        {
            var button = new RadialMenuItem() { Header = launcher.Header };

            button.Click += (s, e) => LauncherService.Launch(launcher);

            return button;
        }
    }
}
