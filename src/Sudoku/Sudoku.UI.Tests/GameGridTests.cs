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
            var grid = new GameGrid();
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
            var grid = new GameGrid();
            bool eventRaised = false;
            grid.PropertyChanged += (sender, args) => eventRaised = true;

            grid[0, 0] = 1;

            Assert.IsTrue(eventRaised);
            Assert.AreEqual(1, grid[0, 0]);
        }

        [TestMethod]
        public void ShouldPopulateGridWithRandomValuesWhenCreatingNewGame()
        {
            var grid = new GameGrid();
            grid.Reinitialize();

            foreach (var item in grid)
            {
                Assert.IsNotNull(item);
            }
        }

    }
}
