using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using KanbanBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KanbanBoard.Behaviors
{
    public class GridBehavior : Behavior<Grid>
    {
        #region Properties

        public static readonly DependencyProperty BoardLayoutProperty =
            DependencyProperty.Register("BoardLayout", typeof(BoardLayout), typeof(GridBehavior), new PropertyMetadata(new PropertyChangedCallback(BoardLayoutChanged)));
        public static readonly DependencyProperty BoarderStyleProperty =
            DependencyProperty.Register("BoarderStyle", typeof(Style), typeof(GridBehavior));
        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register("LineStyle", typeof(Style), typeof(GridBehavior));

        private static void BoardLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GridBehavior).ApplyGridLayout();
        }

        public BoardLayout BoardLayout
        {
            get { return (BoardLayout)GetValue(BoardLayoutProperty); }
            set { SetValue(BoardLayoutProperty, value); }
        }
        public Style BoarderStyle
        {
            get { return (Style)GetValue(BoarderStyleProperty); }
            set { SetValue(BoarderStyleProperty, value); }
        }
        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        private Grid HeadersGrid { get; set; }
        private Grid BackgroundGrid { get; set; }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            HeadersGrid = AssociatedObject.FindName("headerGrid") as Grid;
            BackgroundGrid = AssociatedObject.FindName("colorGrid") as Grid;
        }

        public void ApplyGridLayout()
        {
            CleanupLayout();

            LayoutBackgroundGrid();
            LayoutHeadersGrid();
        }

        private void LayoutHeadersGrid()
        {
            int firstColumnIndex = 0;
            foreach (var column in BoardLayout.Columns)
            {
                for (int i = 0; i < column.Width; i++)
                {
                    HeadersGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1D, GridUnitType.Star) });
                }
                HeadersGrid.Children.Add(CreateNewHeader(firstColumnIndex, column));

                firstColumnIndex += column.Width;
            }
        }

        private void LayoutBackgroundGrid()
        {
            for (int i = 0; i < BoardLayout.RowsCount; i++)
            {
                BackgroundGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1D, GridUnitType.Star) });
            }

            int firstColumnIndex = 0;
            foreach (var column in BoardLayout.Columns)
            {
                for (int i = 0; i < column.Width; i++)
                {
                    BackgroundGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1D, GridUnitType.Star) });
                }
                BackgroundGrid.Children.Add(CreateNewBorder(firstColumnIndex, column));
                if (firstColumnIndex != 0)
                    BackgroundGrid.Children.Add(CreateNewSeparator(firstColumnIndex, column));
                firstColumnIndex += column.Width;
            }
        }

        private void CleanupLayout()
        {
            BackgroundGrid.RowDefinitions.Clear();
            BackgroundGrid.ColumnDefinitions.Clear();
            BackgroundGrid.Children.Clear();

            HeadersGrid.RowDefinitions.Clear();
            HeadersGrid.ColumnDefinitions.Clear();
            HeadersGrid.Children.Clear();
        }

        private BoardColumnHeaderView CreateNewHeader(int columnIndex, BoardColumnDescription column)
        {
            BoardColumnHeaderView header = new BoardColumnHeaderView();
            header.DataContext = column;
            Grid.SetColumn(header, columnIndex);
            Grid.SetColumnSpan(header, column.Width);
            header.Background = new SolidColorBrush(column.BackgroundColor);

            return header;
        }

        private Border CreateNewBorder(int firstColumnIndex, BoardColumnDescription column)
        {
            Border border = new Border();
            border.Style = BoarderStyle;
            border.SetValue(SettingsViewModel.ColumnColorProperty, new SolidColorBrush(column.BackgroundColor));
            Grid.SetColumn(border, firstColumnIndex);
            Grid.SetColumnSpan(border, column.Width);
            Grid.SetRowSpan(border, BoardLayout.RowsCount);

            return border;
        }

        private Line CreateNewSeparator(int columnIndex, BoardColumnDescription column)
        {
            Line line = new Line();
            line.Style = LineStyle;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(line, columnIndex);
            Grid.SetRowSpan(line, BoardLayout.RowsCount);

            return line;
        }
    }
}
