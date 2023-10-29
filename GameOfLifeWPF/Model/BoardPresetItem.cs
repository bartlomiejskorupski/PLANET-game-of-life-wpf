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
        public int StructureWidth {  get; set; }
        public int StructureHeight { get; set; }
        public string ListString
        {
            get
            {
                if(StructureWidth <= 0 || StructureHeight <= 0)
                {
                    return Name;
                }
                return $"{Name} ({StructureWidth}x{StructureHeight})";
            }
        }
        public Type BoardFactoryType{ get; set; }
        public BoardPresetItem(string name, Type boardFactoryType, int width, int height)
        {
            Name = name;
            BoardFactoryType = boardFactoryType;
            StructureWidth = width;
            StructureHeight = height;
        }
        public BoardPresetItem(string name, Type boardFactoryType) : this(name, boardFactoryType, 0, 0) { }
    }
}
