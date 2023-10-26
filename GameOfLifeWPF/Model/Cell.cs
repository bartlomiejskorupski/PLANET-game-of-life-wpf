using System;
using System.Collections.Generic;
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
        public Cell() : this(false) { }
    }
}
