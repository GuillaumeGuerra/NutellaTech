using Framework;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KanbanBoard.ViewModels
{
    public class AvatarViewModel : ViewModel,IToolboxItem
    {
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register("Avatar", typeof(Avatar), typeof(AvatarViewModel));

        public Avatar Avatar
        {
            get { return (Avatar)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }
    }
}
