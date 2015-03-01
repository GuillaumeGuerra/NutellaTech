using Framework.Extensions;
using KanbanBoard.ViewModels;
using KanbanBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace KanbanBoard.Behaviors
{
    public class BoardOverviewDragDropBehavior : AbstractDragDropBehavior<WhiteBoardViewModel, Rectangle>
    {
        private int InitialZoomAreaLeft { get; set; }
        private int InitialZoomAreaTop { get; set; }

        public BoardOverviewDragDropBehavior()
            : base(false) { }

        protected override bool DoMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Canvas canvas = VisualTreeHelperExtensions.GetParent<Canvas>(AssociatedObject);
            BoardOverviewView view = VisualTreeHelperExtensions.GetParent<BoardOverviewView>(AssociatedObject);

            InitialLeftPos = e.GetPosition(canvas).X;
            InitialTopPos = e.GetPosition(canvas).Y;

            InitialZoomAreaLeft = Convert.ToInt32(view.ZoomAreaLeft);
            InitialZoomAreaTop = Convert.ToInt32(view.ZoomAreaTop);

            return true;
        }

        protected override bool DoMouseMove(MouseEventArgs e)
        {
            Canvas canvas = VisualTreeHelperExtensions.GetParent<Canvas>(AssociatedObject);
            BoardOverviewView view = VisualTreeHelperExtensions.GetParent<BoardOverviewView>(AssociatedObject);

            view.ZoomAreaTop = Math.Max(0, Math.Min(InitialZoomAreaTop + Convert.ToInt32(e.GetPosition(canvas).Y - InitialTopPos), canvas.ActualHeight - view.ZoomAreaHeight));
            view.ZoomAreaLeft = Math.Max(0, Math.Min(InitialZoomAreaLeft + Convert.ToInt32(e.GetPosition(canvas).X - InitialLeftPos), canvas.ActualWidth - view.ZoomAreaWidth));

            view.RequestedBoardVerticalOffset = view.ZoomAreaTop * view.BoardMaxVerticalOffset / (view.ActualHeight - view.ZoomAreaHeight);
            view.RequestedBoardHorizontalOffset = view.ZoomAreaLeft * view.BoardMaxHorizontalOffset / (view.ActualWidth - view.ZoomAreaWidth);

            return true;
        }

        protected override bool DoMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            return true;
        }
    }
}
