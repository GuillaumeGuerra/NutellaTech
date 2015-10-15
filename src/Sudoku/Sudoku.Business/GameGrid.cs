using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sudoku.Business.Annotations;

namespace Sudoku.Business
{
    public class GameGrid : IEnumerable<GameGridCell>
    {
        private static Random _random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
        private GameGridCell[] Items { get; set; }

        public int GridSize { get; set; }
        public int AreaSize { get; set; }

        public event Action<int, int> OnValueChanged;

        public GameGrid(int gridSize)
        {
            GridSize = gridSize;
            AreaSize = GridSize / 3;

            // Checking that the grid size is a multiple of 3, to define proper areas
            if (GridSize - 3 * (AreaSize) > 0)
                throw new NotSupportedException("GridSize should be a multiple of 3");

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
                    this[row, column] = 1 + ((row / 3 + column + row * AreaSize) % GridSize);
                }
            }

            // let's make a random number of permutations of rows
            // the permutation have to remain inside the same area, to save consistency
            var rowsOperations = 15 + _random.Next(15);
            for (int operation = 0; operation < rowsOperations; operation++)
            {
                var areaRow = _random.Next(3) * AreaSize;

                var firstAreaRow = areaRow + _random.Next(AreaSize);
                var secondAreaRow = areaRow + _random.Next(AreaSize);

                for (int column = 0; column < GridSize; column++)
                {
                    var temp = this[firstAreaRow, column];

                    this[firstAreaRow, column] = this[secondAreaRow, column];
                    this[secondAreaRow, column] = temp;
                }
            }

            // same for columns
            var columnOperations = 15 + _random.Next(15);
            for (int operation = 0; operation < columnOperations; operation++)
            {
                var areaColumn = _random.Next(3) * AreaSize;

                var firstAreaColumn = areaColumn + _random.Next(AreaSize);
                var secondAreaColumn = areaColumn + _random.Next(AreaSize);

                for (int row = 0; row < GridSize; row++)
                {
                    var temp = this[row, firstAreaColumn];

                    this[row, firstAreaColumn] = this[row, secondAreaColumn];
                    this[row, secondAreaColumn] = temp;
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
            for (int areaRow = 0; areaRow < 3; areaRow++)
            {
                for (int areaColumn = 0; areaColumn < 3; areaColumn++)
                {
                    HashSet<int> values = new HashSet<int>();

                    for (int row = areaRow * AreaSize; row < AreaSize; row++)
                    {
                        for (int column = areaColumn * AreaSize; column < AreaSize; column++)
                        {
                            if (!check(values, row, column))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int row = 0; row < GridSize; row++)
            {
                builder.AppendLine(string.Join(" | ", Enumerable.Range(row * GridSize, GridSize).Select(i =>
                {
                    var cell = Items[i].CurrentValue;
                    return cell.HasValue ? cell.Value.ToString("00") : "  "; // Two digits so that a 12 x 12 grid is well formatted (some values are on 2 digits)
                })));
            }

            return builder.ToString();
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
    }
}