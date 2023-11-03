

namespace GameOfLifeWPF.Model.BoardFactory;

public class LWSSBoardFactory : StructureBoardFactory
{
    protected override int[,] Structure => new int[,]
    {
        { 1,0,0,1,0 },
        { 0,0,0,0,1 },
        { 1,0,0,0,1 },
        { 0,1,1,1,1 },
    };
    protected override int StructureWidth => 5;
    protected override int StructureHeight => 4;
    public LWSSBoardFactory(int width, int height) : base(width, height) { }
}
