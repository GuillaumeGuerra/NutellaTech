using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Framework.Extensions
{
    public static class VisualTreeHelperExtensions
    {
        public static TParent GetParent<TParent>(DependencyObject control)
            where TParent : DependencyObject
        {
            DependencyObject parent = control;
            while (!(parent is TParent))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TParent;
        }
    }
}
