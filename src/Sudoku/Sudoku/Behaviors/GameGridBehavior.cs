using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sudoku.Business;
using UI.Framework.Behaviors;

namespace Sudoku.Behaviors
{
    public class GameGridBehavior : BehaviorBase<Grid>
    {
        public static readonly DependencyProperty GameGridProperty = DependencyProperty.Register("GameGrid", typeof(GameGrid), typeof(GameGridBehavior), new PropertyMetadata(default(GameGrid), GameGridDependencyPropertyChanged));
        private  readonly  Dictionary<Tuple<int,int>,TextBlock> _textBlocks=new Dictionary<Tuple<int, int>, TextBlock>();

        public GameGrid GameGrid
        {
            get { return (GameGrid)GetValue(GameGridProperty); }
            set { SetValue(GameGridProperty, value); }
        }

        private static void GameGridDependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var behavior = dependencyObject as GameGridBehavior;

            if (args.OldValue != null)
                (args.OldValue as GameGrid).OnValueChanged -= behavior.GameGridChanged;

            if (args.NewValue != null)
            {
                var gameGrid = (args.NewValue as GameGrid);
                gameGrid.OnValueChanged += behavior.GameGridChanged;
                behavior.BindItems(gameGrid);
            }
        }

        private void BindItems(GameGrid gameGrid)
        {
            AssociatedElement.RowDefinitions.Clear();
            AssociatedElement.ColumnDefinitions.Clear();
            for (int i = 0; i < gameGrid.GridSize; i++)
            {
                AssociatedElement.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(80) });
                AssociatedElement.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            }

            _textBlocks.Clear();

            gameGrid.ForEach((row, column) =>
            {
                var text = new TextBlock();

                text.VerticalAlignment = VerticalAlignment.Center;
                text.HorizontalAlignment = HorizontalAlignment.Center;
                
                Grid.SetRow(text, row);
                Grid.SetColumn(text, column);

                AssociatedElement.Children.Add(text);

                _textBlocks[new Tuple<int, int>(row, column)] = text;
                GameGridChanged(row, column);
            });
        }

        private void GameGridChanged(int row, int column)
        {
            _textBlocks[new Tuple<int, int>(row, column)].Text = GameGrid.CellAt(row, column).ToString();
        }
    }
}
