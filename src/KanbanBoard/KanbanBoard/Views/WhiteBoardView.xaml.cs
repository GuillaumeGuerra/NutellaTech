using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace KanbanBoard.Views
{
    /// <summary>
    /// Interaction logic for WhiteBoard.xaml
    /// </summary>
    public partial class WhiteBoardView : Window
    {
        public WhiteBoardView()
        {
            InitializeComponent();

            DataContext = new WhiteBoardViewModel();
        }

        public static readonly DependencyProperty RequestedVerticalOffsetProperty =
            DependencyProperty.RegisterAttached("RequestedVerticalOffset", typeof(double), typeof(WhiteBoardView), new PropertyMetadata(0D, new PropertyChangedCallback(BoardVerticalOffsetChanged)));

        public static readonly DependencyProperty RequestedHorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("RequestedHorizontalOffset", typeof(double), typeof(WhiteBoardView), new PropertyMetadata(0D, new PropertyChangedCallback(BoardHorizontalOffsetChanged)));

        private static void BoardHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollViewer).ScrollToHorizontalOffset((double)e.NewValue);
        }

        private static void BoardVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollViewer).ScrollToVerticalOffset((double)e.NewValue);
        }

        [AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
        public static double GetRequestedVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RequestedVerticalOffsetProperty);
        }

        public static void SetRequestedVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RequestedVerticalOffsetProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
        public static double GetRequestedHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RequestedHorizontalOffsetProperty);
        }

        public static void SetRequestedHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RequestedHorizontalOffsetProperty, value);
        }

    }
}
