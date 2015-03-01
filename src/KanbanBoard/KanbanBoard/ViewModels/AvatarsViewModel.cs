using Framework;
using KanbanBoard.Entities;
using KanbanBoard.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KanbanBoard.ViewModels
{
    public class AvatarsViewModel : ViewModel
    {
        public static readonly DependencyProperty AvatarsProperty =
            DependencyProperty.Register("Avatars", typeof(ObservableCollection<AvatarViewModel>), typeof(AvatarsViewModel));

        public ObservableCollection<AvatarViewModel> Avatars
        {
            get { return (ObservableCollection<AvatarViewModel>)GetValue(AvatarsProperty); }
            set { SetValue(AvatarsProperty, value); }
        }

        public AvatarsViewModel()
        {
            Avatars = new ObservableCollection<AvatarViewModel>();
            Avatars.Add(new AvatarViewModel() { Avatar = new Avatar() { Name = "GGU", Image = new Uri("pack://application:,,,/KanbanBoard;component/Resources/Chrysanthemum.jpg") } });
            Avatars.Add(new AvatarViewModel() { Avatar = new Avatar() { Name = "BMS", Image = new Uri("pack://application:,,,/KanbanBoard;component/Resources/Penguins.jpg") } });
            Avatars.Add(new AvatarViewModel() { Avatar = new Avatar() { Name = "JDM", Image = new Uri("pack://application:,,,/KanbanBoard;component/Resources/Lighthouse.jpg") } });
        }
    }
}
