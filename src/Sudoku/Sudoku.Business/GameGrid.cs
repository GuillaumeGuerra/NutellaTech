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
        private int?[] Items { get; } = new int?[9 * 9];

        public event Action<int, int> OnValueChanged;

        public int? this[int row, int column]
        {
            get { return Items[row * 9 + column]; }
            set
            {
                Items[row * 9 + column] = value;
                OnValueChanged?.Invoke(row, column);
            }
        }

        public void Reinitialize()
        {
            int count = 0;
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    this[row, column] = count++;
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
    }
}