using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameOfLifeWPF.Model;

public class BoardState
{
    public HashSet<Point> Cells { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    
    public int Generation { get; set; }
    public int Alive => Cells.Count;
    public int Died { get; set; }
    public int Born { get; set; }
    public BoardState() 
    {
        
    }
    public BoardState(int width, int height, int generation)
    {
        Width = width;
        Height = height;
        Generation = generation;
        Died = 0;
        Born = 0;
        Cells = new HashSet<Point>();
    }

    public BoardState(int width, int height) : this(width, height, 0) { }

    public void ToggleCellState(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException();

        if (IsCellAlive(x, y))
        {
            Cells.Remove(new Point(x, y));
        }
        else
        {
            Cells.Add(new Point(x, y));
        }
    }

    public void SetCellState(int x, int y, bool toState)
    {
        if (x < 0 || x > Width || y < 0 || y > Height)
            throw new ArgumentOutOfRangeException();

        if(toState)
        {
            if(!IsCellAlive(x, y))
            {
                Cells.Add(new Point(x, y));
            }
        }
        else
        {
            Cells.Remove(new Point(x, y));
        }
    }

    public bool IsCellAlive(int x, int y)
    {
        return Cells.Contains(new Point(x, y));
    }

    public BoardState CreateNextBoardState()
    {
        var neightborsDict = CreateNeighborsDict();
        var nextStateCells = CreateNextStateCells(neightborsDict, out int bornChange, out int diedChange);

        var nextState = new BoardState()
        {
            Width = Width,
            Height = Height,
            Generation = Generation + 1,
            Born = Born + bornChange,
            Died = Died + diedChange,
            Cells = nextStateCells
        };

        return nextState;
    }

    private Dictionary<Point, byte> CreateNeighborsDict()
    {
        var neighborsDict = new Dictionary<Point, byte>(Cells.Count * 8);

        foreach (Point cell in Cells)
        {
            for (int offX = -1; offX <= 1; offX++)
            {
                for (int offY = -1; offY <= 1; offY++)
                {
                    if(offX == 0 && offY == 0) continue;

                    if (cell.X + offX < 0 || cell.X + offX >= Width || cell.Y + offY < 0 || cell.Y + offY >= Height)
                        continue;

                    var point = new Point(cell.X + offX, cell.Y + offY);
                    if (neighborsDict.TryGetValue(point, out byte count))
                    {
                        count++;
                        neighborsDict[point] = count;
                    }
                    else
                    {
                        neighborsDict.Add(point, 1);
                    }
                }
            }
        }

        return neighborsDict;
    }

    private HashSet<Point> CreateNextStateCells(Dictionary<Point, byte> neighborsDict, out int bornChange, out int diedChange)
    {
        var nextCells = new HashSet<Point>();
        bornChange = 0;
        diedChange = 0;

        foreach (var pair in neighborsDict)
        {
            var point = pair.Key;
            var neightbors = pair.Value;

            var currCellAlive = IsCellAlive(point.X, point.Y);
            if (currCellAlive)
            {
                if (neightbors == 2 || neightbors == 3)
                {
                    nextCells.Add(point);
                }
                else
                {
                    diedChange++;
                }
            }
            else
            {
                if (neightbors == 3)
                {
                    nextCells.Add(point);
                    bornChange++;
                }
            }
        }

        return nextCells;

    }

}
