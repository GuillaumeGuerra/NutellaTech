using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KanbanBoard.Views
{
    /// <summary>
    /// Interaction logic for BoardOverview.xaml
    /// </summary>
    public partial class BoardOverviewView : UserControl
    {
        public BoardOverviewView()
        {
            InitializeComponent();
        }

        #region Properties

        public static readonly DependencyProperty VisualOverviewProperty =
            DependencyProperty.Register("VisualOverview", typeof(Visual), typeof(BoardOverviewView));
        public static readonly DependencyProperty VisualOverviewWidthProperty =
            DependencyProperty.Register("VisualOverviewWidth", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D, new PropertyChangedCallback(VisualOverviewSizeChanged)));
        public static readonly DependencyProperty VisualOverviewHeightProperty =
            DependencyProperty.Register("VisualOverviewHeight", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D, new PropertyChangedCallback(VisualOverviewSizeChanged)));
        public static readonly DependencyProperty BoardZoomRatioProperty =
            DependencyProperty.Register("BoardZoomRatio", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D, new PropertyChangedCallback(BoardZoomRatioChanged)));
        public static readonly DependencyProperty ZoomAreaLeftProperty =
            DependencyProperty.Register("ZoomAreaLeft", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty ZoomAreaTopProperty =
            DependencyProperty.Register("ZoomAreaTop", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty ZoomAreaWidthProperty =
            DependencyProperty.Register("ZoomAreaWidth", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty ZoomAreaHeightProperty =
            DependencyProperty.Register("ZoomAreaHeight", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty BoardHorizontalOffsetProperty =
            DependencyProperty.Register("BoardHorizontalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D, new PropertyChangedCallback(BoardOffsetChanged)));
        public static readonly DependencyProperty BoardVerticalOffsetProperty =
            DependencyProperty.Register("BoardVerticalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D, new PropertyChangedCallback(BoardOffsetChanged)));
        public static readonly DependencyProperty BoardMaxVerticalOffsetProperty =
            DependencyProperty.Register("BoardMaxVerticalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty BoardMaxHorizontalOffsetProperty =
            DependencyProperty.Register("BoardMaxHorizontalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty RequestedBoardVerticalOffsetProperty =
            DependencyProperty.Register("RequestedBoardVerticalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));
        public static readonly DependencyProperty RequestedBoardHorizontalOffsetProperty =
            DependencyProperty.Register("RequestedBoardHorizontalOffset", typeof(double), typeof(BoardOverviewView), new PropertyMetadata(0D));


        public double RequestedBoardVerticalOffset
        {
            get { return (double)GetValue(RequestedBoardVerticalOffsetProperty); }
            set { SetValue(RequestedBoardVerticalOffsetProperty, value); }
        }

        public double RequestedBoardHorizontalOffset
        {
            get { return (double)GetValue(RequestedBoardHorizontalOffsetProperty); }
            set { SetValue(RequestedBoardHorizontalOffsetProperty, value); }
        }

        private static void VisualOverviewSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoardOverviewView board = d as BoardOverviewView;
            if (board.VisualOverviewHeight == 0D || board.VisualOverviewWidth == 0D)
                return;

            board.Height = board.Width * board.HeightRatio;
        }

        private static void BoardZoomRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoardOverviewView board = d as BoardOverviewView;
            double zoomRatio = (double)e.NewValue;
            if (zoomRatio == 1D)
                return;
            if (((double)e.OldValue) == 1D)
                board.InitialZoomRatio = zoomRatio;

            board.ZoomAreaHeight = board.ActualWidth * board.HeightRatio * board.InitialZoomRatio / zoomRatio;
            board.ZoomAreaWidth = board.ActualWidth * board.InitialZoomRatio / zoomRatio;
        }

        private static void BoardOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoardOverviewView board = d as BoardOverviewView;

            board.ZoomAreaLeft = (board.ActualWidth - board.ZoomAreaWidth) * board.BoardHorizontalOffset / board.BoardMaxHorizontalOffset;
            board.ZoomAreaTop = (board.ActualHeight - board.ZoomAreaHeight) * board.BoardVerticalOffset / board.BoardMaxVerticalOffset;
        }

        public Visual VisualOverview
        {
            get { return (Visual)GetValue(VisualOverviewProperty); }
            set { SetValue(VisualOverviewProperty, value); }
        }
        public double VisualOverviewWidth
        {
            get { return (double)GetValue(VisualOverviewWidthProperty); }
            set { SetValue(VisualOverviewWidthProperty, value); }
        }
        public double VisualOverviewHeight
        {
            get { return (double)GetValue(VisualOverviewHeightProperty); }
            set { SetValue(VisualOverviewHeightProperty, value); }
        }
        public double BoardZoomRatio
        {
            get { return (double)GetValue(BoardZoomRatioProperty); }
            set { SetValue(BoardZoomRatioProperty, value); }
        }
        public double ZoomAreaHeight
        {
            get { return (double)GetValue(ZoomAreaHeightProperty); }
            set { SetValue(ZoomAreaHeightProperty, value); }
        }
        public double ZoomAreaWidth
        {
            get { return (double)GetValue(ZoomAreaWidthProperty); }
            set { SetValue(ZoomAreaWidthProperty, value); }
        }
        public double ZoomAreaTop
        {
            get { return (double)GetValue(ZoomAreaTopProperty); }
            set { SetValue(ZoomAreaTopProperty, value); }
        }
        public double ZoomAreaLeft
        {
            get { return (double)GetValue(ZoomAreaLeftProperty); }
            set { SetValue(ZoomAreaLeftProperty, value); }
        }
        public double BoardHorizontalOffset
        {
            get { return (double)GetValue(BoardHorizontalOffsetProperty); }
            set { SetValue(BoardHorizontalOffsetProperty, value); }
        }
        public double BoardVerticalOffset
        {
            get { return (double)GetValue(BoardVerticalOffsetProperty); }
            set { SetValue(BoardVerticalOffsetProperty, value); }
        }
        public double BoardMaxHorizontalOffset
        {
            get { return (double)GetValue(BoardMaxHorizontalOffsetProperty); }
            set { SetValue(BoardMaxHorizontalOffsetProperty, value); }
        }
        public double BoardMaxVerticalOffset
        {
            get { return (double)GetValue(BoardMaxVerticalOffsetProperty); }
            set { SetValue(BoardMaxVerticalOffsetProperty, value); }
        }

        public double HeightRatio { get { return VisualOverviewHeight / VisualOverviewWidth; } }

        private double InitialZoomRatio { get; set; }

        #endregion
    }
}
