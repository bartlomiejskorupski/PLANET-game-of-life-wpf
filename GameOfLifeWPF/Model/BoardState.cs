using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class BoardState
    {
        public Cell[,] Cells {  get; private set; }
        public int Width { get; }
        public int Height { get; }
        public BoardState(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];
            foreach (var x in Enumerable.Range(0, Width))
            {
                foreach (var y in Enumerable.Range(0, Height))
                {
                    Cells[x, y] = new Cell(false);
                }
            }
        }

        public void ToggleCellState(int x, int y)
        {
            Cells[x, y].IsAlive = !Cells[x, y].IsAlive;
        }

        public void SetCellState(int x, int y, bool state)
        {
            Cells[x, y].IsAlive = state;
        }

        public BoardState CalculateNextState()
        {
            var nextState = new BoardState(Width, Height);

            throw new NotImplementedException();

            return nextState;
        }
    }
}
