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

namespace OneDbgClient.Views
{
    /// <summary>
    /// Interaction logic for DebugProcessView.xaml
    /// </summary>
    public partial class DebugProcessView : UserControl
    {
        public DebugProcessView()
        {
            InitializeComponent();
        }

        private void DebugProcessView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Awful trick to make binding work on the fields
            (this.Resources["ViewModel"] as ObjectDataProvider).ObjectInstance = this.DataContext;
        }
    }
}
