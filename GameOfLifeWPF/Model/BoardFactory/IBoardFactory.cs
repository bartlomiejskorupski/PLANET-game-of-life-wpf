

namespace GameOfLifeWPF.Model.BoardFactory;

public interface IBoardFactory
{
    public Board CreateBoard();
    public bool CanCreate();
}
