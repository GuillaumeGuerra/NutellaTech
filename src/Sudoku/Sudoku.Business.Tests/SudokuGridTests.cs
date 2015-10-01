using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Sudoku.Business.Tests
{
    [TestClass]
    public class SudokuGridTests
    {
        [TestMethod]
        public void GameGridShouldBeEmptyWhenTheGameIsNew()
        {
            var grid = new GameGrid();
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    Assert.IsNull(grid[row, column]);
                }
            }
        }
    }

    public class GameGrid
    {
        private int?[] Items => new int?[9 * 9];

        public int? this[int row, int column] => Items[row*9 + column];
    }
}
