using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class RectCellData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public RectCellData(int x, int y, bool isAlive)
        {
            X = x;
            Y = y;
            IsAlive = isAlive;
        }
    }
}
