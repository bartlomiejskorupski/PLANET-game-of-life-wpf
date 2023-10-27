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
    }
}
