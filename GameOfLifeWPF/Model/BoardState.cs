using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class BoardState
    {
        public Cell[,] Cells { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public int Generation { get; set; }
        [JsonIgnore]
        public int Alive
        {
            get
            {
                int aliveSum = 0;
                foreach(var cell in Cells)
                {
                    aliveSum = cell.IsAlive ? aliveSum + 1 : aliveSum;
                }
                return aliveSum;
            }
        }
        public int Died { get; set; }
        public int Born { get; set; }
        public BoardState() 
        {
            
        }
        public BoardState(int width, int height, int generation)
        {
            Width = width;
            Height = height;
            Generation = generation;
            Died = 0;
            Born = 0;
            Cells = new Cell[width, height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x, y] = new Cell(false);
                }
            }
        }

        public BoardState(int width, int height) : this(width, height, 0) { }

        public void ToggleCellState(int x, int y)
        {
            Cells[x, y].IsAlive = !Cells[x, y].IsAlive;
        }

        public void SetCellState(int x, int y, bool state)
        {
            Cells[x, y].IsAlive = state;
        }

        public BoardState CreateNextBoardState()
        {
            var nextState = new BoardState(Width, Height, Generation + 1);
            nextState.Born = Born;
            nextState.Died = Died;

            for (int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    var nextCell = nextState.Cells[x, y];
                    ChangeCellState(nextCell, x, y);
                    if (nextCell.IsAlive && !Cells[x, y].IsAlive)
                        nextState.Born++;
                    if (!nextCell.IsAlive && Cells[x, y].IsAlive)
                        nextState.Died++;
                }
            }

            return nextState;
        }
        private bool ChangeCellState(Cell nextCell, int x, int y)
        {
            int aliveNeighbors = 0;
            int deadNeighbors = 0;

            for (int offX = -1; offX <= 1; offX++)
            {
                for(int offY = -1; offY <= 1; offY++)
                {
                    if (offX == 0 && offY == 0)
                        continue;

                    if (x + offX < 0 || x + offX >= Width || y + offY < 0 || y + offY >= Height)
                        continue;

                    if(Cells[x + offX, y + offY].IsAlive)
                        aliveNeighbors++;
                    else
                        deadNeighbors++;
                }
            }

            nextCell.IsAlive = false;

            Cell currentCell = Cells[x, y];
            if (currentCell.IsAlive)
            {
                if (aliveNeighbors == 2 || aliveNeighbors == 3)
                {
                    nextCell.IsAlive = true;
                }
            }
            else
            {
                if(aliveNeighbors == 3)
                    nextCell.IsAlive = true;
            }

            return nextCell.IsAlive;
        }
    }
}
