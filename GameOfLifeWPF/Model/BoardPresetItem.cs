using System;

namespace GameOfLifeWPF.Model;

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
    public string ColorString { get; set; }
    public Type BoardFactoryType{ get; set; }
    public BoardPresetItem(string name, Type boardFactoryType, int width, int height, string color = "e3e3e3")
    {
        Name = name;
        BoardFactoryType = boardFactoryType;
        StructureWidth = width;
        StructureHeight = height;
        ColorString = color;
    }
    public BoardPresetItem(string name, Type boardFactoryType, string color = "#e3e3e3") : this(name, boardFactoryType, 0, 0, color) { }
}
