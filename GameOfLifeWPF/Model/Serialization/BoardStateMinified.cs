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
            
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    sb.Append(cells[x, y].IsAlive ? "1" : "0");
                }
            }

            return sb.ToString();
        }

        public static Cell[,] CellsStringToArray(string cellsString, int width, int height)
        {

            char[] splitStr = cellsString.ToCharArray();

            if (splitStr.Length != width * height)
                throw new Exception();

            Cell[,] cells = new Cell[width, height];

            int splitStrCounter = 0;
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool alive = splitStr[splitStrCounter++] == '1';
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
