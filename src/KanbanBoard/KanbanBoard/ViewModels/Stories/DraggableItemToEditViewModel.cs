using Framework;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KanbanBoard.ViewModels
{
    public class DraggableItemToEditViewModel : ViewModel
    {
        public static readonly DependencyProperty DraggableItemProperty =
            DependencyProperty.Register("DraggableItem", typeof(DraggableItemViewModel), typeof(DraggableItemToEditViewModel), new PropertyMetadata(null));
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(Visibility), typeof(DraggableItemToEditViewModel), new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(DraggableItemToEditViewModel), new PropertyMetadata(0D));

        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }
        public DraggableItemViewModel DraggableItem
        {
            get { return (DraggableItemViewModel)GetValue(DraggableItemProperty); }
            set { SetValue(DraggableItemProperty, value); }
        }
        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public RelayCommand HideEditorCommand
        {
            get { return new RelayCommand(HideEditor); }
        }

        private void HideEditor()
        {
            Visibility = Visibility.Collapsed;
            DraggableItem.IsReadOnly = true;
            DraggableItem.QuickActionsVisible = false;
            DraggableItem = null;
        }
    }
}
