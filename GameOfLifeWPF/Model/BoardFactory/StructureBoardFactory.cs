using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.BoardFactory
{
    public abstract class StructureBoardFactory : IBoardFactory
    {
        private readonly int _width;
        private readonly int _height;

        protected abstract int[,] Structure { get; }
        protected abstract int StructureWidth { get; }
        protected abstract int StructureHeight { get; }

        public StructureBoardFactory(int width, int height)
        {
            _width = width;
            _height = height;
        }
        public Board CreateBoard()
        {
            if (!CanCreate())
                return null;

            Board board = new Board(_width, _height);

            CalculateStructurePosition(out int offX, out int offY);

            for (int sX = 0; sX < StructureWidth; sX++)
            {
                for(int sY = 0; sY < StructureHeight; sY++)
                {
                    var alive = Structure[sY, sX] == 1 ? true : false;
                    board.CurrentState.SetCellState(sX + offX, sY + offY, alive);
                }
            }

            return board;
        }

        public bool CanCreate()
        {
            return _width >= StructureWidth && _height >= StructureHeight;
        }

        private void CalculateStructurePosition(out int offX, out int offY)
        {
            offX = (_width - StructureWidth) / 2;
            offY = (_height - StructureHeight) / 2;
        }
    }
}
