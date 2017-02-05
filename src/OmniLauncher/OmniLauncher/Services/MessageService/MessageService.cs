using System;
using System.Windows;

namespace OmniLauncher.Services.IExceptionManager
{
    public class MessageService : IMessageService
    {
        public void ShowException(Exception exception)
        {
            MessageBox.Show(exception.Message, "Unhandled exception", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}