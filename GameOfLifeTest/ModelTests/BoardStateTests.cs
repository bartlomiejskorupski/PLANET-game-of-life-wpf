using GameOfLifeWPF.Model;

namespace GameOfLifeTest.ModelTests
{
    public class BoardStateTests
    {
        public BoardState State { get; set; }
        public BoardStateTests()
        {
            State = new BoardState(10, 10);
        }

        [Fact]
        public void Board_ShouldCreate()
        {
            Assert.NotNull(State);
        }

        [Fact]
        public void Board_ShouldInitializeCellsArray()
        {
            Assert.Equal(State.Width * State.Height, State.Cells.Length);
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(1, 2, false)]
        [InlineData(6, 2, true)]
        [InlineData(9, 9, true)]
        [InlineData(9, 0, false)]
        [InlineData(0, 9, true)]
        public void SetCellState_ShouldChangeCellIsAliveState(int x, int y, bool state)
        {
            State.SetCellState(x, y, state);
            Assert.Equal(state, State.Cells[x, y].IsAlive);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(6, 2)]
        [InlineData(9, 9)]
        [InlineData(9, 0)]
        [InlineData(0, 9)]
        public void ToggleCellState_ShouldToggleCellIsAliveState(int x, int y)
        {
            State.ToggleCellState(x, y);
            Assert.True(State.Cells[x, y].IsAlive);
            State.ToggleCellState(x, y);
            Assert.False(State.Cells[x, y].IsAlive);
            State.ToggleCellState(x, y);
            Assert.True(State.Cells[x, y].IsAlive);
        }

        [Fact]
        public void Alive_ShouldReturnTheNumberOfLivingCells()
        {
            State.SetCellState(4, 4, true);
            State.SetCellState(4, 5, true);
            State.SetCellState(5, 4, true);
            State.SetCellState(5, 5, true);
            State.SetCellState(6, 6, true);
            State.SetCellState(7, 7, true);

            Assert.Equal(6, State.Alive);
        }

        [Fact]
        public void CalculateNextBoardState_ShouldCreateANewInstanceOfBoardState()
        {
            var nextState = State.CreateNextBoardState();
            Assert.NotSame(State, nextState);
        }

        [Fact]
        public void CalculateNextBoardState_WhenBlockCreatureIsOnBoard_ThenItStaysAlive()
        {
            State.SetCellState(4, 4, true);
            State.SetCellState(4, 5, true);
            State.SetCellState(5, 4, true);
            State.SetCellState(5, 5, true);

            var nextState = State.CreateNextBoardState();
            Assert.Equal(4, nextState.Alive);
            Assert.Equal(0, nextState.Died);
            Assert.Equal(0, nextState.Born);
            Assert.True(nextState.Cells[4, 4].IsAlive);
            Assert.True(nextState.Cells[4, 5].IsAlive);
            Assert.True(nextState.Cells[5, 4].IsAlive);
            Assert.True(nextState.Cells[5, 5].IsAlive);
        }

        [Fact]
        public void CalculateNextBoardState_WhenBlinkerCreatureIsOnBoard_ThenItBlinks()
        {
            State.SetCellState(4, 5, true);
            State.SetCellState(5, 5, true);
            State.SetCellState(6, 5, true);

            var nextState = State.CreateNextBoardState();
            Assert.Equal(3, nextState.Alive);
            Assert.Equal(2, nextState.Died);
            Assert.Equal(2, nextState.Born);
            Assert.True(nextState.Cells[5, 4].IsAlive);
            Assert.True(nextState.Cells[5, 5].IsAlive);
            Assert.True(nextState.Cells[5, 6].IsAlive);
            Assert.False(nextState.Cells[4, 5].IsAlive);
            Assert.False(nextState.Cells[6, 5].IsAlive);
        }

        [Fact]
        public void CalculateNextBoardState_WhenBeaconCreatureIsOnBoard_ThenItBlinks()
        {
            State.SetCellState(4, 4, true);
            State.SetCellState(4, 5, true);
            State.SetCellState(5, 4, true);
            State.SetCellState(7, 7, true);
            State.SetCellState(7, 6, true);
            State.SetCellState(6, 7, true);

            var nextState = State.CreateNextBoardState();
            Assert.Equal(8, nextState.Alive);
            Assert.Equal(0, nextState.Died);
            Assert.Equal(2, nextState.Born);
            Assert.True(nextState.Cells[4, 4].IsAlive);
            Assert.True(nextState.Cells[4, 5].IsAlive);
            Assert.True(nextState.Cells[5, 4].IsAlive);
            Assert.True(nextState.Cells[7, 7].IsAlive);
            Assert.True(nextState.Cells[7, 6].IsAlive);
            Assert.True(nextState.Cells[6, 7].IsAlive);
            Assert.True(nextState.Cells[5, 5].IsAlive);
            Assert.True(nextState.Cells[6, 6].IsAlive);

            nextState = nextState.CreateNextBoardState();
            Assert.Equal(6, nextState.Alive);
            Assert.Equal(2, nextState.Died);
            Assert.Equal(2, nextState.Born);
            Assert.True(nextState.Cells[4, 4].IsAlive);
            Assert.True(nextState.Cells[4, 5].IsAlive);
            Assert.True(nextState.Cells[5, 4].IsAlive);
            Assert.True(nextState.Cells[7, 7].IsAlive);
            Assert.True(nextState.Cells[7, 6].IsAlive);
            Assert.True(nextState.Cells[6, 7].IsAlive);
            Assert.False(nextState.Cells[5, 5].IsAlive);
            Assert.False(nextState.Cells[6, 6].IsAlive);
        }
    }
}
