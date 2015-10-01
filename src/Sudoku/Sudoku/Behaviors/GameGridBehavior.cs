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

        private  void BindItems(GameGrid gameGrid)
        {
            throw new NotImplementedException();
        }

        private  void GameGridChanged(int row, int column)
        {
            throw new NotImplementedException();
        }

        public GameGrid GameGrid
        {
            get { return (GameGrid)GetValue(GameGridProperty); }
            set { SetValue(GameGridProperty, value); }
        }
    }
}
