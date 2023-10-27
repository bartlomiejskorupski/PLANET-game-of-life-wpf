using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class Cell
    {
        public bool IsAlive { get; set; }
        public Cell(bool isAlive)
        {
            IsAlive = isAlive;
        }
        public Cell(int x, int y) : this(false) { }
    }
}
