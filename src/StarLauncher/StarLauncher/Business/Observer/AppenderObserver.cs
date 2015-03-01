using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace StarLauncher.Business
{
    public class AppenderObserver : DependencyObject, IObserver
    {
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(AppenderObserver));
        
        public string ProgressText
        {
            get { return (string)GetValue(ProgressTextProperty); }
            set { SetValue(ProgressTextProperty, value); }
        }

        public BackgroundWorker Worker { get; set; }

        public void PushMessage(string message, MessageLevel level)
        {
            Worker.ReportProgress(0, message);
        }
    }
}
