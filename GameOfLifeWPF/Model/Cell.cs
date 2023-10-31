using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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
