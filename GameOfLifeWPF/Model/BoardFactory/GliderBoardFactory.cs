using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.BoardFactory
{
    public class GliderBoardFactory : StructureBoardFactory
    { 
        protected override int[,] Structure => new int[,]
        {
            { 0,1,0 },
            { 0,0,1 },
            { 1,1,1 }
        };
        protected override int StructureWidth => 3;
        protected override int StructureHeight => 3;
        public GliderBoardFactory(int width, int height) : base(width, height) { }
    }
}
