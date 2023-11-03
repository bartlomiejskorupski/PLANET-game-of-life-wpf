

namespace GameOfLifeWPF.Model.BoardFactory;

public class MWSSBoardFactory : StructureBoardFactory
{
    protected override int[,] Structure => new int[,]
    {
        { 0,0,1,0,0,0 },
        { 1,0,0,0,1,0 },
        { 0,0,0,0,0,1 },
        { 1,0,0,0,0,1 },
        { 0,1,1,1,1,1 },
    };
    protected override int StructureWidth => 6;
    protected override int StructureHeight => 5;
    public MWSSBoardFactory(int width, int height) : base(width, height) { }
}
