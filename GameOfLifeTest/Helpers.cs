using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeTest
{
    public static class Helpers
    {
        public static HashSet<Point> CellsArrayToSet(int[,] cellsArr)
        {
            var cells = new HashSet<Point>();

            for(int x = 0; x < cellsArr.GetLength(0); x++)
            {
                for(int y = 0; y < cellsArr.GetLength(1); y++)
                {
                    if (cellsArr[x, y] == 1)
                        cells.Add(new Point(x, y));
                }
            }

            return cells;
        }
    }
}
