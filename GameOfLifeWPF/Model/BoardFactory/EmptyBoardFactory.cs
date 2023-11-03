

namespace GameOfLifeWPF.Model.BoardFactory;

public class EmptyBoardFactory : IBoardFactory
{
    private readonly int _width;
    private readonly int _height;
    public EmptyBoardFactory(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public bool CanCreate()
    {
        return true;
    }

    public Board CreateBoard()
    {
        return new Board(_width, _height);
    }
}
