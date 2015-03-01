using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneNote.View
{
    /// <summary>
    /// Interaction logic for FlatSplitPanel.xaml
    /// </summary>
    public partial class FlatSplitPanel : UserControl
    {
        public FlatSplitPanel()
        {
            InitializeComponent();
        }

        public FrameworkElement LeftControl
        {
            get { return (FrameworkElement)GetValue(LeftControlProperty); }
            set { SetValue(LeftControlProperty, value); }
        }

        public FrameworkElement RightControl
        {
            get { return (FrameworkElement)GetValue(RightControlProperty); }
            set { SetValue(RightControlProperty, value); }
        }

        public static readonly DependencyProperty LeftControlProperty =
            DependencyProperty.Register("LeftControl", typeof(FrameworkElement), typeof(FlatSplitPanel), new PropertyMetadata(null, LeftControlChanged));
        public static readonly DependencyProperty RightControlProperty =
            DependencyProperty.Register("RightControl", typeof(FrameworkElement), typeof(FlatSplitPanel), new PropertyMetadata(null, RightControlChanged));

        private static void LeftControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FlatSplitPanel).SetLeftControl(e.NewValue as FrameworkElement);
        }

        private static void RightControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FlatSplitPanel).SetRightControl(e.NewValue as FrameworkElement);
        }

        private void SetLeftControl(FrameworkElement element)
        {
            this.leftSidePanel.Children.Add(element);
            SetTransparentBackground(element);
        }

        private void SetRightControl(FrameworkElement element)
        {
            this.rightSidePanel.Children.Add(element);
            SetTransparentBackground(element);
        }

        private void SetTransparentBackground(FrameworkElement element)
        {
            if (element is Control)
                (element as Control).Background = new SolidColorBrush(Colors.Transparent);
            if (element is Panel)
                (element as Panel).Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
