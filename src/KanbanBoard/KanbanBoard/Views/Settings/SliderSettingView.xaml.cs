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
    /// Interaction logic for SliderSettingView.xaml
    /// </summary>
    public partial class SliderSettingView : UserControl
    {
        public static readonly DependencyProperty SliderMaxProperty =
            DependencyProperty.Register("SliderMax", typeof(double), typeof(SliderSettingView), new PropertyMetadata(0D));
        public static readonly DependencyProperty SliderMinProperty =
            DependencyProperty.Register("SliderMin", typeof(double), typeof(SliderSettingView), new PropertyMetadata(0D));
        public static readonly DependencyProperty SliderValueProperty =
            DependencyProperty.Register("SliderValue", typeof(double), typeof(SliderSettingView), new PropertyMetadata(0D));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(SliderSettingView));
        public static readonly DependencyProperty SliderTickFrequencyProperty =
            DependencyProperty.Register("SliderTickFrequency", typeof(double), typeof(SliderSettingView), new PropertyMetadata(0.1D));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public double SliderValue
        {
            get { return (double)GetValue(SliderValueProperty); }
            set { SetValue(SliderValueProperty, value); }
        }
        public double SliderMin
        {
            get { return (double)GetValue(SliderMinProperty); }
            set { SetValue(SliderMinProperty, value); }
        }
        public double SliderMax
        {
            get { return (double)GetValue(SliderMaxProperty); }
            set { SetValue(SliderMaxProperty, value); }
        }
        public double SliderTickFrequency
        {
            get { return (double)GetValue(SliderTickFrequencyProperty); }
            set { SetValue(SliderTickFrequencyProperty, value); }
        }

        public SliderSettingView()
        {
            InitializeComponent();
        }
    }
}
