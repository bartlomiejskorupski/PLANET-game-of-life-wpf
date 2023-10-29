using GameOfLifeWPF.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model.BoardFactory
{
    public class LoadBoardFactory : IBoardFactory
    {
        private readonly string _path;
        public LoadBoardFactory(string path)
        {
            _path = path;
        }

        public Board CreateBoard()
        {
            return BoardSerializer.Deserialize(_path);
        }

        public bool CanCreate()
        {
            return true;
        }
    }
}
