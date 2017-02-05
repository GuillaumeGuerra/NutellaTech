using System;
using System.Windows;

namespace OmniLauncher.Services.IExceptionManager
{
    public class ExceptionManager : IExceptionManager
    {
        public void Show(Exception exception)
        {
            MessageBox.Show(exception.Message, "Unhandled exception", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}