using System;
using System.Windows;

namespace OneDbgClient.Framework
{
    public class PopupService : IPopupService
    {
        public void ShowError(string title, string explanation, Exception exception)
        {
            MessageBox.Show(App.Current.MainWindow, explanation + Environment.NewLine + "Exception : " + Environment.NewLine + exception, title, 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowError(string title, Exception exception)
        {
            MessageBox.Show(App.Current.MainWindow, "Exception : " + Environment.NewLine + exception, title,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInformation(string title, string text)
        {
            MessageBox.Show(App.Current.MainWindow, title, text, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}