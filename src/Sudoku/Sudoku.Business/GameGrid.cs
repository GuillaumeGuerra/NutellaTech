using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sudoku.Business.Annotations;

namespace Sudoku.Business
{
    public class GameGrid : IEnumerable<int?>
    {
        private static Random _random = new Random((int) (DateTime.Now.Ticks % int.MaxValue));

        public int GridSize { get; set; }
        private int?[] Items { get; set; }

        public event Action<int, int> OnValueChanged;

        public GameGrid(int gridSize)
        {
            GridSize = gridSize;
            Items = new int?[GridSize * GridSize];
        }

        public int? this[int row, int column]
        {
            get { return Items[row * GridSize + column]; }
            set
            {
                Items[row * GridSize + column] = value;
                OnValueChanged?.Invoke(row, column);
            }
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

        #region Interfaces

        IEnumerator<int?> IEnumerable<int?>.GetEnumerator()
        {
            return (IEnumerator<int?>)Items.GetEnumerator();
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

                  if (item.Value < 1 || item.Value > GridSize)
                      return false; // value out of range

                  if (set.Contains(item.Value))
                      return false;

                  set.Add(item.Value);

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

            return true;
        }
    }
}