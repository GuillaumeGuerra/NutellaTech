using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sudoku.Business.Annotations;

namespace Sudoku.Business
{
    public class GameGrid : IEnumerable<GameGridCell>
    {
        private static Random _random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

        public int GridSize { get; set; }
        private GameGridCell[] Items { get; set; }

        public event Action<int, int> OnValueChanged;

        public GameGrid(int gridSize)
        {
            // Checking that the grid size is a multiple of 3, to define proper areas
            if (gridSize - 3 * (gridSize / 3) > 0)
                throw new NotSupportedException("GridSize should be a multiple of 3");

            GridSize = gridSize;
            Items = new GameGridCell[GridSize * GridSize];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new GameGridCell();
            }
        }

        public int? this[int row, int column]
        {
            get { return CellAt(row, column).CurrentValue; }
            set
            {
                CellAt(row, column).CurrentValue = value;
                OnValueChanged?.Invoke(row, column);
            }
        }

        public GameGridCell CellAt(int row, int column)
        {
            return Items[row * GridSize + column];
        }

        public void Reinitialize()
        {
            // First, populate all cells with a valid layout

            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    this[row, column] = 1 + ((column + row) % GridSize);
                }
            }

            // let's make a random number of permutations of rows
            var rowsOperations = 15 + _random.Next(15);
            for (int i = 0; i < rowsOperations; i++)
            {
                var firstRow = _random.Next(GridSize);
                var secondRow = _random.Next(GridSize);

                for (int j = 0; j < GridSize; j++)
                {
                    var temp = this[firstRow, j];

                    this[firstRow, j] = this[secondRow, j];
                    this[secondRow, j] = temp;
                }
            }

            // same for columns
            var columnOperations = 15 + _random.Next(15);
            for (int i = 0; i < columnOperations; i++)
            {
                var firstColumn = _random.Next(GridSize);
                var secondColumn = _random.Next(GridSize);

                for (int j = 0; j < GridSize; j++)
                {
                    var temp = this[j, firstColumn];

                    this[j, firstColumn] = this[j, secondColumn];
                    this[j, secondColumn] = temp;
                }
            }
        }

        public void ForEach(Action<int, int> action)
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    action(row, column);
                }
            }
        }

        #region Interfaces

        IEnumerator<GameGridCell> IEnumerable<GameGridCell>.GetEnumerator()
        {
            return (IEnumerator<GameGridCell>)Items.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        public bool IsValid()
        {
            Func<HashSet<int>, int, int, bool> check = (set, row, column) =>
              {
                  var item = this[row, column];
                  if (!item.HasValue)
                      return true;

                  var value = item.Value;
                  if (value < 1 || value > GridSize)
                      return false; // value out of range

                  if (set.Contains(value))
                      return false;

                  set.Add(value);

                  return true;
              };

            // Checking rows
            for (int row = 0; row < GridSize; row++)
            {
                HashSet<int> values = new HashSet<int>();
                for (int column = 0; column < GridSize; column++)
                {
                    if (!check(values, row, column))
                        return false;
                }
            }

            // Checking columns
            for (int column = 0; column < GridSize; column++)
            {
                HashSet<int> values = new HashSet<int>();
                for (int row = 0; row < GridSize; row++)
                {
                    if (!check(values, row, column))
                        return false;
                }
            }

            // Checking areas
            var areaSize = GridSize / 3;
            for (int areaRow = 0; areaRow < 3; areaRow++)
            {
                for (int areaColumn = 0; areaColumn < 3; areaColumn++)
                {
                    HashSet<int> values = new HashSet<int>();

                    for (int row = areaRow * areaSize; row < areaSize; row++)
                    {
                        for (int column = areaColumn * areaSize; column < areaSize; column++)
                        {
                            if (!check(values, row, column))
                                return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}