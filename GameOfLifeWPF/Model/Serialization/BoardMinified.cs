using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.Serialization
{
    public class BoardMinified
    {
        [JsonProperty(PropertyName = "s")]
        public IList<BoardStateMinified> States { get; set; }
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }
        public BoardMinified()
        {

        }
        public BoardMinified(Board board)
        {
            Width = board.Width;
            Height = board.Height;
            States = new List<BoardStateMinified>();
            foreach (var state in board.States)
            {
                States.Add(new BoardStateMinified(state));
            }
        }

        public Board ToBoard()
        {
            var board = new Board();
            board.Width = Width;
            board.Height = Height;

            foreach (var stateMini in States)
            {
                board.States.Add(stateMini.ToBoardState(Width, Height));
            }

            return board;
        }
    }
}
