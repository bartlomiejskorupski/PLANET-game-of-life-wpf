using Newtonsoft.Json;
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
        public int Width { get; set; }
        public int Height { get; set; }
        [JsonIgnore]
        public int CurrentStateId => States.Count - 1;
        [JsonIgnore]
        public BoardState CurrentState => States[CurrentStateId];
        [JsonIgnore]
        public string GenerationText => CurrentState.Generation.ToString();
        [JsonIgnore]
        public string AliveText => CurrentState.Alive.ToString();
        [JsonIgnore]
        public string DiedText => CurrentState.Died.ToString();
        [JsonIgnore]
        public string BornText => CurrentState.Born.ToString();
        public Board()
        {
            States = new List<BoardState>();
        }
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            States = new List<BoardState>()
            {
                new BoardState(Width, Height)
            };
        }

        public void NextGeneration()
        {
            var nextState = CurrentState.CreateNextBoardState();
            States.Add(nextState);
        }

        public void StepBack()
        {
            if(CurrentStateId <= 0)
            {
                return;
            }
            States.RemoveAt(CurrentStateId);
        }

    }
}
