using Framework;
using GalaSoft.MvvmLight.Command;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KanbanBoard.ViewModels
{
    public abstract class DraggableItemViewModel : ViewModel, IDraggableItem
    {
        #region Properties

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(DraggableItemViewModel));
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(DraggableItemViewModel));
        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(int), typeof(DraggableItemViewModel));
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(int), typeof(DraggableItemViewModel));
        public static readonly DependencyProperty ZIndexProperty =
            DependencyProperty.Register("ZIndex", typeof(int), typeof(DraggableItemViewModel));
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DraggableItemViewModel), new PropertyMetadata(IsReadOnlyPropertyChanged));
        public static readonly DependencyProperty QuickActionsVisibleProperty =
            DependencyProperty.Register("QuickActionsVisible", typeof(bool), typeof(DraggableItemViewModel), new PropertyMetadata(false));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(DraggableItemViewModel), new PropertyMetadata(0));
        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register("RotateAngle", typeof(double), typeof(DraggableItemViewModel), new PropertyMetadata(0D));
        public static readonly DependencyProperty RotateAngleRangeProperty =
            DependencyProperty.Register("RotateAngleRange", typeof(double), typeof(DraggableItemViewModel), new PropertyMetadata(3D, new PropertyChangedCallback(RotateAngleRangeChanged)));
        public static readonly DependencyProperty RotateAngleFactorProperty =
            DependencyProperty.Register("RotateAngleFactor", typeof(double), typeof(DraggableItemViewModel), new PropertyMetadata(0D));
        public static readonly DependencyProperty HightlightStatusProperty =
            DependencyProperty.Register("HightlightStatus", typeof(HightlightStatus), typeof(DraggableItemViewModel), new PropertyMetadata(HightlightStatus.Hightlighted));

        private static void IsReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.OldValue != (bool)e.NewValue)
                (d as DraggableItemViewModel).IsReadOnlyValueChanged(e);
        }

        private static void RotateAngleRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DraggableItemViewModel vm = d as DraggableItemViewModel;
            vm.RotateAngle = vm.RotateAngleFactor * (double)e.NewValue;
        }

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public int Top
        {
            get { return (int)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
        public int Left
        {
            get { return (int)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public int ZIndex
        {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public bool QuickActionsVisible
        {
            get { return (bool)GetValue(QuickActionsVisibleProperty); }
            set { SetValue(QuickActionsVisibleProperty, value); }
        }
        public double RotateAngle
        {
            get { return (double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }
        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public double RotateAngleRange
        {
            get { return (double)GetValue(RotateAngleRangeProperty); }
            set { SetValue(RotateAngleRangeProperty, value); }
        }
        public double RotateAngleFactor
        {
            get { return (double)GetValue(RotateAngleFactorProperty); }
            set { SetValue(RotateAngleFactorProperty, value); }
        }
        public HightlightStatus HightlightStatus
        {
            get { return (HightlightStatus)GetValue(HightlightStatusProperty); }
            set { SetValue(HightlightStatusProperty, value); }
        }

        public Guid ItemKey { get; set; }

        protected UserStoriesViewModel AllStories { get; set; }

        #endregion

        #region Commands

        public RelayCommand DeleteAvatarCommand
        {
            get { return new RelayCommand(DeleteAvatar); }
        }

        public RelayCommand CloseEditModeCommand
        {
            get { return new RelayCommand(CloseEditMode); }
        }

        public RelayCommand ShowItemEditorCommand
        {
            get { return new RelayCommand(ShowItemEditor); }
        }

        public RelayCommand AssignTodayToStartDateCommand
        {
            get { return new RelayCommand(AssignTodayToStartDate); }
        }

        public RelayCommand AssignTodayToDevDoneDateCommand
        {
            get { return new RelayCommand(AssignTodayToDevDoneDate); }
        }

        public RelayCommand AssignTodayToEndDateCommand
        {
            get { return new RelayCommand(AssignTodayToEndDate); }
        }

        #endregion

        public DraggableItemViewModel(UserStoriesViewModel allStories)
        {
            AllStories = allStories;
            ItemKey = Guid.NewGuid();
        }

        private void CloseEditMode()
        {
            IsReadOnly = true;
            QuickActionsVisible = false;
        }

        private void ShowItemEditor()
        {
            AllStories.ShowEditor(this);
        }

        protected virtual void IsReadOnlyValueChanged(DependencyPropertyChangedEventArgs e) { }

        public abstract void DropToolboxItem(IToolboxItem item);

        public abstract void DeleteAvatar();
        public abstract void AssignTodayToStartDate();
        public abstract void AssignTodayToDevDoneDate();
        public abstract void AssignTodayToEndDate();

        public abstract string Status { get; set; }
        public abstract int Index { get; set; }
    }
}
