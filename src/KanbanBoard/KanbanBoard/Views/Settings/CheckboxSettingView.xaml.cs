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
    public partial class CheckboxSettingView : UserControl
    {
        public static readonly DependencyProperty CheckboxValueProperty =
            DependencyProperty.Register("CheckboxValue", typeof(bool), typeof(CheckboxSettingView));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(CheckboxSettingView));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public bool CheckboxValue
        {
            get { return (bool)GetValue(CheckboxValueProperty); }
            set { SetValue(CheckboxValueProperty, value); }
        }

        public CheckboxSettingView()
        {
            InitializeComponent();
        }
    }
}
