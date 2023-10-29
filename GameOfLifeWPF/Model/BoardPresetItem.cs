using GameOfLifeWPF.Model.BoardFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeWPF.Model
{
    public class BoardPresetItem
    {
        public string Name { get; set; }
        public Type BoardFactoryType{ get; set; }
        public BoardPresetItem(string name, Type boardFactoryType)
        {
            Name = name;
            BoardFactoryType = boardFactoryType;
        }
    }
}
