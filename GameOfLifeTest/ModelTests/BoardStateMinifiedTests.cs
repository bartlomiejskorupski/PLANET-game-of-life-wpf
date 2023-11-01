using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeTest.ModelTests
{
    public class BoardStateMinifiedTests
    {

        [Fact]
        public void CellsArrayToString_When4x4_ShouldCreateMinifiedCellsString()
        {
            var cellsArr = new int[4, 4]
            {
                { 1,1,0,0  },
                { 0,1,0,1  },
                { 0,0,0,0  },
                { 0,1,0,0  },
            };
            var cells = Helpers.CellsArrayToSet(cellsArr);

            var cellsString = BoardStateMinified.CellsArrayToString(cells, 4, 4);
            Assert.Equal("C504", cellsString);
        }

        [Fact]
        public void CellsArrayToString_When7x4_ShouldCreateMinifiedCellsString()
        {
            var cellsArr = new int[7, 4]
            {
                { 1,1,0,0 },
                { 0,1,0,1 },
                { 0,0,0,0 },
                { 0,1,0,0 },
                { 0,1,0,0 },
                { 0,1,0,0 },
                { 0,1,0,0 },
            };
            var cells = Helpers.CellsArrayToSet(cellsArr);

            var cellsString = BoardStateMinified.CellsArrayToString(cells, 7, 4);
            Assert.Equal("C5044404", cellsString);
        }

        [Fact]
        public void CellsStringToArray_When7x4_ShouldCreateMinifiedCellsString()
        {
            var cells = BoardStateMinified.CellsStringToArray("C5044404", 7, 4);
            Assert.Contains(new Point(0, 0), cells);
            Assert.Contains(new Point(0, 1), cells);
            Assert.DoesNotContain(new Point(0, 2), cells);
            Assert.DoesNotContain(new Point(0, 3), cells);
            Assert.DoesNotContain(new Point(4, 0), cells);
            Assert.Contains(new Point(4, 1), cells);
            Assert.DoesNotContain(new Point(4, 2), cells);
            Assert.DoesNotContain(new Point(4, 3), cells);
        }

    }
}
