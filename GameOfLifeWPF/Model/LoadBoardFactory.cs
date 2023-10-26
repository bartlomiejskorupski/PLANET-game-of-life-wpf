using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class LoadBoardFactory: IBoardFactory
    {
        private readonly string _path;
        public LoadBoardFactory(string path)
        {
            _path = path;
        }

        public Board CreateBoard()
        {
            throw new NotImplementedException();
        }
    }
}
