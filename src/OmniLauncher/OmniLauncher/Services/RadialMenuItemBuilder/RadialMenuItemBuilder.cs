using System.Collections.ObjectModel;
using Infragistics.Controls.Menus;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.LauncherService;

namespace OmniLauncher.Services.RadialMenuItemBuilder
{
    public class RadialMenuItemBuilder : IRadialMenuItemBuilder
    {
        public ILauncherService LauncherService { get; set; }

        public ObservableCollection<RadialMenuItem> BuildMenuItems(LaunchersNode launchers)
        {
            var items = new ObservableCollection<RadialMenuItem>();

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
            var item = new RadialMenuItem {Header = launchers.Header};

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