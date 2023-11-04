using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace GameOfLifeWPF.Model;

public class RectangleViewModel
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Left { get; set; }
    public double Top { get; set; }
    public Brush Fill { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public RectangleViewModel()
    {
        Width = 0;
        Height = 0;
        Left = 0;
        Top = 0;
        Fill = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        X = -1;
        Y = -1;
    }
}
