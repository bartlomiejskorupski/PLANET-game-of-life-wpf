using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.Serialization;
using System;
using System.Collections.Generic;
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
            var cells = new Cell[4, 4]
            {
                { new Cell(true), new Cell(true), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(true),  },
                { new Cell(), new Cell(), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(),  },
            };

            var cellsString = BoardStateMinified.CellsArrayToString(cells);
            Assert.Equal("C504", cellsString);
        }

        [Fact]
        public void CellsArrayToString_When7x4_ShouldCreateMinifiedCellsString()
        {
            var cells = new Cell[7, 4]
            {
                { new Cell(true), new Cell(true), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(true),  },
                { new Cell(), new Cell(), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(),  },
                { new Cell(), new Cell(true), new Cell(), new Cell(),  },
            };

            var cellsString = BoardStateMinified.CellsArrayToString(cells);
            Assert.Equal("C5044404", cellsString);
        }

        [Fact]
        public void CellsStringToArray_When7x4_ShouldCreateMinifiedCellsString()
        {
            var cells = BoardStateMinified.CellsStringToArray("C5044404", 7, 4);
            Assert.Equal(7, cells.GetLength(0));
            Assert.Equal(4, cells.GetLength(1));
            Assert.True(cells[0, 0].IsAlive);
            Assert.True(cells[0, 1].IsAlive);
            Assert.False(cells[0, 2].IsAlive);
            Assert.False(cells[0, 3].IsAlive);
            Assert.False(cells[4, 0].IsAlive);
            Assert.True(cells[4, 1].IsAlive);
            Assert.False(cells[4, 2].IsAlive);
            Assert.False(cells[4, 3].IsAlive);
        }

    }
}
