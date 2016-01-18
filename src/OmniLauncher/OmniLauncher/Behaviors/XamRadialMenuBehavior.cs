using System;
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
            AssociatedObject.Items.Clear();
            foreach (var rootGroup in Launchers.RootGroups)
            {
                AssociatedObject.Items.Add(new RadialMenuItem() { Header = rootGroup.Header });
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
