using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KanbanBoard.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public static readonly DependencyProperty DragDropOpacityProperty =
            DependencyProperty.Register("DragDropOpacity", typeof(double), typeof(SettingsViewModel), new PropertyMetadata(0.5D));
        public static readonly DependencyProperty UserStoryZoomRatioProperty =
            DependencyProperty.Register("UserStoryZoomRatio", typeof(double), typeof(SettingsViewModel), new PropertyMetadata(1.3D));
        public static readonly DependencyProperty ActivateMagnifierProperty =
            DependencyProperty.Register("ActivateMagnifier", typeof(bool), typeof(SettingsViewModel), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(SettingsViewModel), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowColumnsProperty =
            DependencyProperty.Register("ShowColumns", typeof(bool), typeof(SettingsViewModel), new PropertyMetadata(true));
        public static readonly DependencyProperty ColumnColorProperty =
            DependencyProperty.RegisterAttached("ColumnColor", typeof(Brush), typeof(SettingsViewModel));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(SettingsViewModel), new PropertyMetadata(0));
        public static readonly DependencyProperty RotateAngleFactorProperty =
            DependencyProperty.Register("RotateAngleFactor", typeof(double), typeof(SettingsViewModel), new PropertyMetadata(1D));

        public static Brush GetColumnColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ColumnColorProperty);
        }

        public static void SetColumnColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(ColumnColorProperty, value);
        }

        public double DragDropOpacity
        {
            get { return (double)GetValue(DragDropOpacityProperty); }
            set { SetValue(DragDropOpacityProperty, value); }
        }
        public double UserStoryZoomRatio
        {
            get { return (double)GetValue(UserStoryZoomRatioProperty); }
            set { SetValue(UserStoryZoomRatioProperty, value); }
        }
        public bool ActivateMagnifier
        {
            get { return (bool)GetValue(ActivateMagnifierProperty); }
            set { SetValue(ActivateMagnifierProperty, value); }
        }
        public bool ShowGridLines
        {
            get { return (bool)GetValue(ShowGridLinesProperty); }
            set { SetValue(ShowGridLinesProperty, value); }
        }
        public bool ShowColumns
        {
            get { return (bool)GetValue(ShowColumnsProperty); }
            set { SetValue(ShowColumnsProperty, value); }
        }
        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public double RotateAngleFactor
        {
            get { return (double)GetValue(RotateAngleFactorProperty); }
            set { SetValue(RotateAngleFactorProperty, value); }
        }
    }
}
