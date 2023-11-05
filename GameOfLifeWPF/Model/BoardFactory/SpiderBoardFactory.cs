

namespace GameOfLifeWPF.Model.BoardFactory;


public class SpiderBoardFactory : StructureBoardFactory
{
    protected override int[,] Structure => new int[,]
    {
        { 0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,1,1,0,1,0,1,0,1,1,0,0,0,1,1,0,1,0,1,0,1,1,0,0,0 },
        { 1,1,1,0,1,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,1,0,1,1,1 },
        { 1,0,0,0,1,0,1,0,0,0,0,0,1,0,1,0,0,0,0,0,1,0,1,0,0,0,1 },
        { 0,0,0,0,1,1,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,1,1,0,0,0,0 },
        { 0,1,1,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,1,1,0 },
        { 0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,0 },
        { 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0 }
    };
    protected override int StructureWidth => 27;
    protected override int StructureHeight => 8;
    public SpiderBoardFactory(int width, int height) : base(width, height) { }
}