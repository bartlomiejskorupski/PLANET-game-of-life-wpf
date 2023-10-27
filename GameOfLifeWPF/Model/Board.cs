using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GameOfLifeWPF.Model
{
    public class Board
    {
        public IList<BoardState> States { get; set; }
        public int Width { get; }
        public int Height { get; }
        public int CurrentStateId { get; set; }
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            States = new List<BoardState>()
            {
                new BoardState(Width, Height)
            };
            CurrentStateId = 0;
        }

    }
}
