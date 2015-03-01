using Framework;
using KanbanBoard.Entities;
using KanbanBoard.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock;

namespace KanbanBoard.ViewModels
{
    public class ToolboxAdorner : Adorner, IAdorner
    {
        public ToolboxAdorner(FrameworkElement element)
            : base(element)
        {
            Element = element;
            Element.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(element_MouseMove), true);
            Element.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(element_MouseUp), true);
        }

        #region Dependency Properties

        public static readonly DependencyProperty ToolboxItemProperty =
            DependencyProperty.Register("ToolboxItem", typeof(IToolboxItem), typeof(ToolboxAdorner));
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ToolboxAdorner));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public IToolboxItem ToolboxItem
        {
            get { return (IToolboxItem)GetValue(ToolboxItemProperty); }
            set { SetValue(ToolboxItemProperty, value); }
        }

        #endregion

        #region Properties

        private Size Boundary { get; set; }
        protected override int VisualChildrenCount { get { return 1; } }
        protected override Visual GetVisualChild(int index) { return Canvas; }
        private Canvas Canvas { get; set; }
        private FrameworkElement Element { get; set; }
        private Grid Grid { get; set; }

        #endregion

        #region Public Methods

        public void Detach()
        {
            Element.RemoveHandler(FrameworkElement.MouseMoveEvent, new MouseEventHandler(element_MouseMove));
            Element.RemoveHandler(FrameworkElement.MouseUpEvent, new MouseButtonEventHandler(element_MouseUp));
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Element);
            adornerLayer.Remove(this);
        }

        public void Initialize()
        {
            var children = new VisualCollection(this);
            Canvas = new Canvas();
            children.Add(Canvas);

            CreateToolboxControl();
            UpdatePosition();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Element);
            adornerLayer.Add(this);
        }

        #endregion

        #region Internal Methods

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);
            adornedElementRect.Inflate(Boundary);
            Canvas.Arrange(adornedElementRect);
            return finalSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Canvas.Measure(constraint);
            return Canvas.DesiredSize;
        }

        private void element_MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePosition();
        }

        private void element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var res = VisualTreeHelper.HitTest(this.Element, Mouse.GetPosition(this.AdornedElement));
            DraggableItemViewModel vm = GetHighestDraggableItem(res.VisualHit);
            if (vm != null)
            {
                vm.DropToolboxItem(ToolboxItem);
            }

            IsActive = false;
            Detach();
        }

        private void CreateToolboxControl()
        {
            if (Grid != null)
            {
                Canvas.Children.Remove(Grid);
            }

            Grid = new Grid();

            var listview = new ListView();
            listview.Background = Brushes.AliceBlue;
            listview.ItemsSource = new List<IToolboxItem>() { ToolboxItem };
            Grid.Children.Add(listview);

            Canvas.Children.Add(Grid);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Point currentMousePosition = Mouse.GetPosition(this.AdornedElement);

            Canvas.SetLeft(Grid, currentMousePosition.X);
            Canvas.SetTop(Grid, currentMousePosition.Y);
        }

        private DraggableItemViewModel GetHighestDraggableItem(DependencyObject control)
        {
            DraggableItemViewModel highest = null;
            DependencyObject current = control;
            while (true)
            {
                if (current == null)
                    break;

                FrameworkElement element = current as FrameworkElement;
                if (element != null && element.DataContext is DraggableItemViewModel)
                    highest = element.DataContext as DraggableItemViewModel;

                current = VisualTreeHelper.GetParent(current);

            }
            return highest;
        }

        #endregion
    }
}
