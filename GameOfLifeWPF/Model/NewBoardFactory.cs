using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class NewBoardFactory : IBoardFactory
    {
        private readonly int _width;
        private readonly int _height;
        public NewBoardFactory(int width, int height)
        {
            _width = width;
            _height = height;
        }
        public Board CreateBoard()
        {
            return new Board(_width, _height);
        }
    }
}
