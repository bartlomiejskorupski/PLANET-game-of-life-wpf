﻿using GameOfLifeWPF.Model.Serialization;
using System;

namespace GameOfLifeWPF.Model.BoardFactory;

public class LoadBoardFactory : IBoardFactory
{
    private readonly string _path;
    private Board? LoadedBoard { get; set; }
    public LoadBoardFactory(string path)
    {
        _path = path;
        LoadedBoard = null;
    }

    public Board CreateBoard()
    {
        return LoadedBoard!;
    }

    public bool CanCreate()
    {
        try
        {
            LoadedBoard = BoardSerializer.Deserialize(_path);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
