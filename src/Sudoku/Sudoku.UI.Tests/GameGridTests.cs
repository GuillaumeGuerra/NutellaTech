using System;
using System.Collections.Generic;
using System.Linq;
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
        public void ShouldCreateValidGridWithRandomValuesWhenCreatingNewGame()
        {
            List<GameGrid> grids = new List<GameGrid>();

            for (int count = 0; count < 10; count++)
            {
                var grid = new GameGrid(6);
                grid.Reinitialize();

                foreach (var item in grid)
                {
                    Assert.IsNotNull(item);
                }

                Assert.IsTrue(grid.IsValid(), "Grids should be valid");

                grids.Add(grid);
            }

            for (int leftIndex = 0; leftIndex < grids.Count; leftIndex++)
            {
                for (int rightIndex = 0; rightIndex < grids.Count; rightIndex++)
                {
                    var left = grids[leftIndex];
                    var right = grids[rightIndex];

                    if (object.ReferenceEquals(left, right))
                        continue;

                    Assert.IsFalse(CompareGrids(left, right), "Grids should not be the same (indexes {0},{1})", leftIndex, rightIndex);
                }
            }
        }

        [TestMethod]
        public void ShouldValidateGridWhenGridIsEmpty()
        {
            var grid = new GameGrid(3);
            Assert.IsTrue(grid.IsValid());
        }

        [TestMethod]
        public void ShouldValidateGridWhenNoDuplicateIsFoundOnRowsColumnsAndAreas()
        {
            var grid = new GameGrid(6);
            grid[0, 0] = 1;
            grid[0, 1] = 2;
            grid[0, 2] = 3;
            grid[0, 3] = 4;
            grid[0, 4] = 5;
            grid[0, 5] = 6;

            grid[1, 0] = 2;
            grid[1, 1] = 3;
            grid[1, 2] = 4;
            grid[1, 3] = 5;
            grid[1, 4] = 6;
            grid[1, 5] = 1;

            grid[2, 0] = 3;
            grid[2, 1] = 4;
            grid[2, 2] = 5;
            grid[2, 3] = 6;
            grid[2, 4] = 1;
            grid[2, 5] = 2;

            grid[3, 0] = 4;
            grid[3, 1] = 5;
            grid[3, 2] = 6;
            grid[3, 3] = 1;
            grid[3, 4] = 2;
            grid[3, 5] = 3;

            grid[4, 0] = 5;
            grid[4, 1] = 6;
            grid[4, 2] = 1;
            grid[4, 3] = 2;
            grid[4, 4] = 3;
            grid[4, 5] = 4;

            grid[5, 0] = 6;
            grid[5, 1] = 1;
            grid[5, 2] = 2;
            grid[5, 3] = 3;
            grid[5, 4] = 4;
            grid[5, 5] = 5;

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
        [DataRow(0, 0)]
        [DataRow(0, 1)]
        [DataRow(0, 2)]
        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        public void ShouldNotValidateGridWhenDuplicateIsFoundInSameArea(int areaRow, int areaColumn)
        {
            var grid = new GameGrid(6);
            grid[2 * areaRow + 0, 2 * areaColumn + 0] = 1;
            grid[2 * areaRow + 0, 2 * areaColumn + 1] = 2;
            grid[2 * areaRow + 1, 2 * areaColumn + 0] = 3;
            grid[2 * areaRow + 1, 2 * areaColumn + 1] = 3; // Duplicate

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

        [TestMethod]
        public void ShouldRefuseToCreateGameGridWhenGridSizeIsNotAMultipleOf3()
        {
            Assert.ThrowsException<NotSupportedException>(() => new GameGrid(4));
        }

        public bool CompareGrids(GameGrid expected, GameGrid actual)
        {
            Assert.AreEqual(expected.GridSize, actual.GridSize);

            for (int row = 0; row < expected.GridSize; row++)
            {
                for (int column = 0; column < expected.GridSize; column++)
                {
                    if (expected[row, column] != actual[row, column])
                        return false;
                }
            }

            return true;
        }
    }
}
