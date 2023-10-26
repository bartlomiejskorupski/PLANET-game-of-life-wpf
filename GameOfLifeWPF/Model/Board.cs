using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class Board
    {
        public Cell[,] Cells { get; set; }
        public Board(int width, int height)
        {
            Cells = new Cell[width, height];
        }
    }
}
