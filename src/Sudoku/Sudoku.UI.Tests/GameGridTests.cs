using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Sudoku.Business;

namespace Sudoku.UI.Tests
{
    [TestClass]
    public class GameGridTests
    {
        [TestMethod]
        public void GameGridShouldBeEmptyWhenTheGameIsNew()
        {
            var grid = new GameGrid(9);
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    Assert.IsNull(grid[row, column]);
                }
            }
        }

        [TestMethod]
        public void ShouldRaideOnPropertyChangedWhenValueIsChanged()
        {
            var grid = new GameGrid(9);
            bool eventRaised = false;
            grid.OnValueChanged += (sender, args) => eventRaised = true;

            grid[0, 0] = 1;

            Assert.IsTrue(eventRaised);
            Assert.AreEqual(1, grid[0, 0]);
        }

        [TestMethod]
        public void ShouldPopulateGridWithRandomValuesWhenCreatingNewGame()
        {
            var grid = new GameGrid(6);
            grid.Reinitialize();

            foreach (var item in grid)
            {
                Assert.IsNotNull(item);
            }
        }

        [TestMethod]
        public void ShouldValidateGridWhenGridIsEmpty()
        {
            var grid = new GameGrid(3);
            Assert.IsTrue(grid.IsValid());
        }

        [TestMethod]
        public void ShouldValidateGridWhenNoDuplicateIsFoundOnEachRowAndColumn()
        {
            var grid = new GameGrid(3);
            grid[0, 0] = 1;
            grid[0, 1] = 2;
            grid[0, 2] = 3;
            grid[1, 0] = 2;
            grid[1, 1] = 3;
            grid[1, 2] = 1;
            grid[2, 0] = 3;
            grid[2, 1] = 1;
            grid[2, 2] = 2;

            Assert.IsTrue(grid.IsValid());
        }

        [TestMethod]
        public void ShouldNotValidateGridWhenDuplicateIsFoundOnOneRow()
        {
            var grid = new GameGrid(3);
            grid[0, 0] = 1;
            grid[0, 1] = 2;
            grid[0, 2] = 2; // Duplicate

            Assert.IsFalse(grid.IsValid());
        }

        [TestMethod]
        public void ShouldNotValidateGridWhenDuplicateIsFoundOnOneColumn()
        {
            var grid = new GameGrid(3);
            grid[0, 0] = 1;
            grid[1, 0] = 2;
            grid[2, 0] = 2; // Duplicate

            Assert.IsFalse(grid.IsValid());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(4)]
        public void ShouldNotValidateGridWhenValueOutOfRangeIsFound(int outOfRangeValue)
        {
            var grid = new GameGrid(3);
            grid[0, 0] = 1;
            grid[1, 0] = 2;
            grid[2, 0] = outOfRangeValue; // Out of range

            Assert.IsFalse(grid.IsValid());
        }
    }
}
