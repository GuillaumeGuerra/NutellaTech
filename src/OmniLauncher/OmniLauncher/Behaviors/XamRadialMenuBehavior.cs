using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Infragistics.Controls.Menus;
using Infragistics.Windows.Internal;
using Microsoft.Practices.ServiceLocation;
using OmniLauncher.Services.IExceptionManager;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.ViewModels;

namespace OmniLauncher.Behaviors
{
    public class XamRadialMenuBehavior : Behavior<XamRadialMenu>
    {
        public static readonly DependencyProperty LaunchersProperty =
            DependencyProperty.Register("Launchers", typeof(Launchers), typeof(XamRadialMenuBehavior), new PropertyMetadata(PropertyChangedCallback));

        public Launchers Launchers
        {
            get { return (Launchers)GetValue(LaunchersProperty); }
            set { SetValue(LaunchersProperty, value); }
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

            var items = GetRadialMenuItems(Launchers);

            AssociatedObject.Items.Clear();
            foreach (var item in items)
            {
                AssociatedObject.Items.Add(item);
            }

        }

        public IList GetRadialMenuItems(Launchers launchers)
        {
            var items = new List<object>();

            if (launchers != null)
            {
                foreach (var rootGroup in launchers.RootGroups)
                {
                    items.Add(GetMenuItem(rootGroup));
                }
            }

            return items;
        }

        private static RadialMenuItem GetMenuItem(LaunchersRootGroup rootGroup)
        {
            var item = new RadialMenuItem() { Header = rootGroup.Header };

            foreach (var group in rootGroup.Groups)
            {
                item.Items.Add(GetMenuItem(@group));
            }

            return item;
        }

        private static RadialMenuItem GetMenuItem(LaunchersGroup group)
        {
            var item = new RadialMenuItem() { Header = @group.Header };

            foreach (var launcher in group.Launchers)
            {
                var button = new RadialMenuItem() { Header = launcher.Header };

                button.Click += (s, e) =>
                {
                    try
                    {
                        Process.Start(launcher.Command);
                    }
                    catch (Exception exception)
                    {
                        ServiceLocator.Current.GetInstance<IExceptionManager>().Show(exception);
                    }
                };

                item.Items.Add(button);
            }

            return item;
        }
    }
}
