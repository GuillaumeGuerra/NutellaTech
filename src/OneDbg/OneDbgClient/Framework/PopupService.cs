using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace OneDbgClient.Framework
{
    public class PopupService : IPopupService
    {
        public async void ShowError(string title, string explanation, Exception exception = null)
        {
            string exceptionText = "";
            if (exception != null)
                exceptionText = Environment.NewLine + Environment.NewLine + "Exception : " + Environment.NewLine + exception;

            await GetMetroWindow().ShowMessageAsync(title, explanation + exceptionText, MessageDialogStyle.Affirmative,
                new MetroDialogSettings(){AnimateHide = true,AnimateShow = true});
        }

        private static MetroWindow GetMetroWindow()
        {
            return App.Current.MainWindow as MetroWindow;
        }

        public async void ShowError(string title, Exception exception)
        {
            await GetMetroWindow().ShowMessageAsync(title, "Exception : " + Environment.NewLine + exception, MessageDialogStyle.Affirmative,
                new MetroDialogSettings() { AnimateHide = true, AnimateShow = true });
        }

        public async void ShowInformation(string title, string text)
        {
            await GetMetroWindow().ShowMessageAsync(title, text, MessageDialogStyle.Affirmative,
                new MetroDialogSettings() { AnimateHide = true, AnimateShow = true });
        }
    }
}