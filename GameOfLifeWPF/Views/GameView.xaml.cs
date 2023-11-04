﻿using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
using GameOfLifeWPF.Model.ScreenCapture;
using GameOfLifeWPF.Model.Serialization;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GameOfLifeWPF.Views;


public partial class GameView : UserControl
{
    public Board Board { get; set; }
    public ObservableCollection<RectangleViewModel> Cells { get; set; }

    private DispatcherTimer _autoTimer;
    private DispatcherTimer _resizeTimer;

    public GameView(IBoardFactory boardFactory)
    {
        InitializeComponent();
        Cells = new ObservableCollection<RectangleViewModel>();

        Board = boardFactory.CreateBoard();
        DataContext = Board;

        _autoTimer = new DispatcherTimer(DispatcherPriority.Render);
        _autoTimer.Interval = TimeSpan.FromMilliseconds(200);
        _autoTimer.Tick += AutoTimerTick;

        _resizeTimer = new DispatcherTimer();
        _resizeTimer.Tick += ResizeTimerTick;
        _resizeTimer.Interval = TimeSpan.FromMilliseconds(500);
    }

    public void GameCanvas_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

        Cells.Clear();
        GameCanvas.ItemsSource = null;

        var currentState = Board.CurrentState;
        // Canvas background
        Cells.Add(new RectangleViewModel()
        {
            Width = cellSize * currentState.Width,
            Height = cellSize * currentState.Height,
            Left = -offsetLeft,
            Top = -offsetTop,
            Fill = new SolidColorBrush(Color.FromRgb(24, 24, 24))
        });

        foreach(var cell in currentState.Cells)
        {
            Cells.Add(new RectangleViewModel()
            {
                Width = cellSize,
                Height = cellSize,
                Left = cellSize * cell.X - offsetLeft,
                Top = cellSize * cell.Y - offsetTop,
                X = cell.X,
                Y = cell.Y
            });
        }

        GameCanvas.ItemsSource = Cells;
    }

    private void CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop)
    {
        double contWidth = GameCanvas.ActualWidth;
        double contHeight = GameCanvas.ActualHeight;

        double containerRatio = contWidth / contHeight;
        double boardRatio = Board.Width / Board.Height;

        if (containerRatio >= boardRatio)
        {
            cellSize = contHeight / Board.Height;
            offsetLeft = cellSize * Board.Width / 2.0;
            offsetTop = contHeight / 2.0;
        }
        else
        {
            cellSize = contWidth / Board.Width;
            offsetLeft = contWidth / 2.0;
            offsetTop = cellSize * Board.Height / 2.0;
        }
    }

    private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var itemsControl = (ItemsControl)sender;
        CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

        var canvasWidth = cellSize * Board.Width;
        var canvasHeight = cellSize * Board.Height;

        var icWidth = itemsControl.ActualWidth;
        var icHeight = itemsControl.ActualHeight;

        var widthDiff = icWidth - canvasWidth;
        var heightDiff = icHeight - canvasHeight;

        var wDiffHalf = widthDiff / 2.0;
        var hDiffHalf = heightDiff / 2.0;

        var clickPoint = e.GetPosition(itemsControl);
        var cellX = (int)((clickPoint.X - wDiffHalf) / cellSize);
        var cellY = (int)((clickPoint.Y - hDiffHalf) / cellSize);

        if (cellX < 0 || cellY < 0 || cellX >= Board.Width || cellY >= Board.Height)
            return;

        var currentState = Board.CurrentState;
        if(currentState.IsCellAlive(cellX, cellY))
        {
            var rectVM = Cells.Where(vm => vm.X == cellX && vm.Y == cellY).First();
            Cells.Remove(rectVM);
        }
        else
        {
            Cells.Add(new RectangleViewModel()
            {
                X = cellX,
                Y = cellY,
                Width = cellSize,
                Height = cellSize,
                Left = cellSize * cellX - offsetLeft,
                Top = cellSize * cellY - offsetTop,
            });
        }

        currentState.ToggleCellState(cellX, cellY);
        AliveTB.Text = Board.AliveText;
    }

    private void DebugButton_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        _resizeTimer.Start();
    }

    private async void NextButton_Click(object sender, RoutedEventArgs e)
    {
        await Task.Run(Board.NextGeneration);
        UpdateTopPanel();
        UpdateCanvas();
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        Board.StepBack();
        UpdateTopPanel();
        UpdateCanvas();
    }

    private void UpdateTopPanel()
    {
        GenerationTB.Text = Board.GenerationText;
        AliveTB.Text = Board.AliveText;
        BornTB.Text = Board.BornText;
        DiedTB.Text = Board.DiedText;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to exit?\nUnsaved changes will be discarded.",
            "Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No
        );
        if (result != MessageBoxResult.Yes)
            return;

        _resizeTimer.Stop();
        _autoTimer.Stop();

        Board = null;
        DataContext = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForFullGCComplete();

        App.Navigate(new TitleView());
    }

    private void AutoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_autoTimer == null) return;

        var slider = (Slider)sender;
        int value = (int)e.NewValue;
        int max = (int)slider.Maximum;

        if (value == max)
        {
            _autoTimer.Stop();
            return;
        }

        if (value <= 0) throw new InvalidOperationException();

        _autoTimer.Interval = TimeSpan.FromMilliseconds(value);

        if (!_autoTimer.IsEnabled)
        {
            _autoTimer.Start();
        }
    }

    private async void AutoTimerTick(object? sender, EventArgs e)
    {
        _autoTimer.Stop();
        await Task.Run(Board.NextGeneration);
        UpdateTopPanel();
        UpdateCanvas();
        _autoTimer.Start();
    }

    private void ResizeTimerTick(object? sender, EventArgs e)
    {
        _resizeTimer.Stop();
        UpdateCanvas();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.Filter = "Game of life save file (.gol)|*.gol";
        dialog.FileName = "GOL_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".gol";

        if (dialog.ShowDialog() == false)
            return;

        BoardSerializer.Serialize(Board, dialog.FileName);
    }

    private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "JPEG image (.jpeg)|*.jpeg";
        dialog.FileName = "GOL_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg";

        if (dialog.ShowDialog() == false)
            return;

        Screenshot.SaveUIElementToJpeg(GameGrid, dialog.FileName);

        Process.Start("rundll32.exe", "shell32.dll,OpenAs_RunDLL " + dialog.FileName);
    }

}
