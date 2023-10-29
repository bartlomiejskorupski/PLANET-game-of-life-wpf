using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.BoardFactory
{
    public class PulsarBoardFactory : StructureBoardFactory
    {
        protected override int[,] Structure => new int[,]
        {
            { 0,0,1,1,1,0,0,0,1,1,1,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 0,0,1,1,1,0,0,0,1,1,1,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,1,1,0,0,0,1,1,1,0,0 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 1,0,0,0,0,1,0,1,0,0,0,0,1 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,1,1,0,0,0,1,1,1,0,0 },
        };
        protected override int StructureWidth => 13;
        protected override int StructureHeight => 13;
        public PulsarBoardFactory(int width, int height) : base(width, height) { }
    }
}
