using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Framework
{
    public static class AnimationUtilities
    {
        public static void LaunchAnimation(Control control ,string storyboardName)
        {
            Storyboard storyboard = control.FindResource(storyboardName) as Storyboard;
            LaunchAnimation(control, storyboard);
        }

        public static void LaunchAnimation(FrameworkElement control, Storyboard storyboard)
        {
            storyboard.Begin(control);
        }
    }
}
