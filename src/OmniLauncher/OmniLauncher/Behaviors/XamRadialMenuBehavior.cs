using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Infragistics.Controls.Menus;
using Infragistics.Windows.Internal;
using OmniLauncher.Services.LauncherConfigurationProcessor;

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

        private void ApplyItems()
        {
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
                    items.Add(new RadialMenuItem() { Header = rootGroup.Header });
                }
            }

            return items;
        }
    }
}
