using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public interface IBoardFactory
    {
        public Board CreateBoard();
    }
}
