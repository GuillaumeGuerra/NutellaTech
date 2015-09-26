using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace OneDbgClient.Framework
{
    public class MessageService : IMessageService
    {
        private  MetroDialogSettings _metroDialogSettings;

        public MessageService()
        {
            _metroDialogSettings = new MetroDialogSettings()
            {
                AnimateHide = true,
                AnimateShow = true,
                CustomResourceDictionary =
                    new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml")
                    },
                SuppressDefaultResources = true
            };
        }

        public async void ShowError(string title, string explanation, Exception exception = null)
        {
            string exceptionText = "";
            if (exception != null)
                exceptionText = Environment.NewLine + Environment.NewLine + "Exception : " + Environment.NewLine + exception;

            await GetMetroWindow().ShowMessageAsync(title, explanation + exceptionText, MessageDialogStyle.Affirmative,
                _metroDialogSettings);
        }

        public async void ShowError(string title, Exception exception)
        {
            await GetMetroWindow().ShowMessageAsync(title, "Exception : " + Environment.NewLine + exception, MessageDialogStyle.Affirmative,
                _metroDialogSettings);
        }

        public async void ShowInformation(string title, string text)
        {
            await GetMetroWindow().ShowMessageAsync(title, text, MessageDialogStyle.Affirmative,
                _metroDialogSettings);
        }

        private  MetroWindow GetMetroWindow()
        {
            return App.Current.MainWindow as MetroWindow;
        }
    }
}