using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoard.Entities;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;
using KanbanBoard.Views;
using System.Windows.Threading;
using System.Threading;

namespace KanbanBoard.ViewModels
{
    public class WhiteBoardViewModel : ViewModel
    {
        #region Binding

        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register("Settings", typeof(SettingsViewModel), typeof(WhiteBoardViewModel));
        public static readonly DependencyProperty StoriesProperty =
            DependencyProperty.Register("Stories", typeof(UserStoriesViewModel), typeof(WhiteBoardViewModel));
        public static readonly DependencyProperty BoardDragDropProperty =
            DependencyProperty.Register("BoardDragDrop", typeof(BoardViewModel), typeof(WhiteBoardViewModel));
        public static readonly DependencyProperty AvatarsProperty =
            DependencyProperty.Register("Avatars", typeof(AvatarsViewModel), typeof(WhiteBoardViewModel));

        public RelayCommand WindowLoadedCommand
        {
            get { return new RelayCommand(WindowLoaded); }
        }

        public SettingsViewModel Settings
        {
            get { return (SettingsViewModel)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        public UserStoriesViewModel Stories
        {
            get { return (UserStoriesViewModel)GetValue(StoriesProperty); }
            set { SetValue(StoriesProperty, value); }
        }

        public BoardViewModel BoardDragDrop
        {
            get { return (BoardViewModel)GetValue(BoardDragDropProperty); }
            set { SetValue(BoardDragDropProperty, value); }
        }

        public AvatarsViewModel Avatars
        {
            get { return (AvatarsViewModel)GetValue(AvatarsProperty); }
            set { SetValue(AvatarsProperty, value); }
        }

        #endregion

        public WhiteBoardViewModel()
        {
            Settings = new SettingsViewModel();
            Stories = new UserStoriesViewModel();
            BoardDragDrop = new BoardViewModel(Stories);
            Avatars = new AvatarsViewModel();

            Application.Current.Resources.Add("Settings", Settings);
        }

        #region Other Commands

        private void WindowLoaded()
        {
            Stories.InitializeStories();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => BoardDragDrop.ResizeFullScreen()));
        }

        #endregion
    }
}
