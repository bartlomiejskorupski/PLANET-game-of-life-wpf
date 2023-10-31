using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.Serialization
{
    public class BoardStateMinified
    {
        [JsonProperty(PropertyName = "c")]
        public string Cells { get; set; }
        [JsonProperty(PropertyName = "g")]
        public int Generation { get; set; }
        [JsonProperty(PropertyName = "d")]
        public int Died { get; set; }
        [JsonProperty(PropertyName = "b")]
        public int Born { get; set; }
        public BoardStateMinified()
        {
            
        }
        public BoardStateMinified(BoardState state)
        {
            Generation = state.Generation;
            Died = state.Died;
            Born = state.Born;

            Cells = CellsArrayToString(state.Cells);
        }

        public static string CellsArrayToString(Cell[,] cells)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            StringBuilder sb = new StringBuilder();

            byte cellsByte = 0;
            int bitCounter = 0;
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    var cell = cells[x, y];
                    cellsByte <<= 1;
                    if (cell.IsAlive)
                        cellsByte |= 0b_0000_0001;
                    bitCounter++;
                    if (bitCounter >= 8)
                    {
                        sb.Append(cellsByte.ToString("X2"));
                        cellsByte = 0;
                        bitCounter = 0;
                    }
                }
            }

            if (bitCounter != 0)
                sb.Append(cellsByte.ToString("X2"));

            return sb.ToString();
        }

        public static Cell[,] CellsStringToArray(string cellsString, int width, int height)
        {
            if (cellsString.Length < width * height / 4)
                throw new Exception();

            Cell[,] cells = new Cell[width, height];

            byte currByte = 0;
            int byteCounter = 0;
            int substrCounter = 0;
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(byteCounter <= 0)
                    {
                        currByte = Convert.ToByte(cellsString.Substring(substrCounter, 2), 16);
                        substrCounter += 2;
                        byteCounter = 8;
                    }

                    bool alive = (currByte & 0b_1000_0000) == 0b_1000_0000;

                    currByte <<= 1;
                    byteCounter--;

                    cells[x, y] = new Cell(alive);
                }
            }

            return cells;
        }

        public BoardState ToBoardState(int width, int height)
        {
            BoardState state = new BoardState();
            state.Width = width;
            state.Height = height;
            state.Generation = Generation;
            state.Died = Died;
            state.Born = Born;
            state.Cells = CellsStringToArray(Cells, width, height);
            return state;
        }
    }
}
