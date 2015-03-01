using Framework;
using GalaSoft.MvvmLight.Command;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Serialization;

namespace KanbanBoard.ViewModels
{
    public class BoardViewModel : ViewModel
    {
        public static readonly DependencyProperty BoardZoomRatioProperty =
            DependencyProperty.Register("BoardZoomRatio", typeof(double), typeof(BoardViewModel), new PropertyMetadata(1D));
        public static readonly DependencyProperty AreaWidthProperty =
            DependencyProperty.Register("AreaWidth", typeof(double), typeof(WhiteBoardViewModel));
        public static readonly DependencyProperty AreaHeightProperty =
            DependencyProperty.Register("AreaHeight", typeof(double), typeof(WhiteBoardViewModel));

        public RelayCommand ResizeFullScreenCommand
        {
            get { return new RelayCommand(ResizeFullScreen); }
        }

        public double BoardZoomRatio
        {
            get { return (double)GetValue(BoardZoomRatioProperty); }
            set { SetValue(BoardZoomRatioProperty, value); }
        }

        public double AreaWidth
        {
            get { return (double)GetValue(AreaWidthProperty); }
            set { SetValue(AreaWidthProperty, value); }
        }

        public double AreaHeight
        {
            get { return (double)GetValue(AreaHeightProperty); }
            set { SetValue(AreaHeightProperty, value); }
        }

        private UserStoriesViewModel Stories { get; set; }

        public BoardViewModel(UserStoriesViewModel stories)
        {
            Stories = stories;
        }

        #region Button Commands

        public void ResizeFullScreen()
        {
            BoardZoomRatio = GetFullScreenZoomRatio();
        }

        #endregion

        #region Other Methods

        public double GetFullScreenZoomRatio()
        {
            return Math.Min((AreaWidth - 20) / Stories.CanvasWidth, (AreaHeight - 20) / Stories.CanvasHeight);
        }

        #endregion
    }
}
