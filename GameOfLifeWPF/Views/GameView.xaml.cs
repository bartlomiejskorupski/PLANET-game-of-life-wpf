using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
using GameOfLifeWPF.Model.ScreenCapture;
using GameOfLifeWPF.Model.Serialization;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public HashSet<System.Drawing.Point>? CellsBefore { get; set; }

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
        //GameCanvas.ItemsSource = null;
        
        if(CellsBefore == null)
        {
            InitCanvas();
            CellsBefore = Board.CurrentState.Cells;
        }
        else
        {
            AddChangesToCanvas();
        }
        
        if(GameCanvas.ItemsSource == null)
            GameCanvas.ItemsSource = Cells;
    }

    private void InitCanvas()
    {
        CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

        Cells.Clear();
        var currentState = Board.CurrentState;
        // Add background
        Cells.Add(new RectangleViewModel()
        {
            Width = cellSize * currentState.Width,
            Height = cellSize * currentState.Height,
            Left = -offsetLeft,
            Top = -offsetTop,
            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(24, 24, 24))
        });

        foreach (var cell in currentState.Cells)
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
    }

    private void AddChangesToCanvas()
    {
        if (CellsBefore == null) throw new InvalidOperationException();

        CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

        var currentCells = Board.CurrentState.Cells;

        foreach (var cell in CellsBefore)
        {
            if (!currentCells.Contains(cell))
            {
                var rect = Cells.Where(rect => rect.X == cell.X && rect.Y == cell.Y).FirstOrDefault();
                if(rect != null) Cells.Remove(rect);
            }
        }

        foreach (var cell in currentCells)
        {
            if (!CellsBefore.Contains(cell))
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
        }

        CellsBefore = currentCells;
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
        ResetAutoSlider();
        await Task.Run(Board.NextGeneration);
        UpdateTopPanel();
        UpdateCanvas();
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        ResetAutoSlider();
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
        ResetAutoSlider();

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
        if(AutoSlider.Value != AutoSlider.Maximum)
            _autoTimer.Start();
    }

    private void ResetAutoSlider()
    {
        AutoSlider.Value = AutoSlider.Maximum;
    }

    private void ResizeTimerTick(object? sender, EventArgs e)
    {
        _resizeTimer.Stop();
        CellsBefore = null;
        UpdateCanvas();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        ResetAutoSlider();

        var dialog = new SaveFileDialog();
        dialog.Filter = "Game of life save file (.gol)|*.gol";
        dialog.FileName = "GOL_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".gol";

        if (dialog.ShowDialog() == false)
            return;

        try
        {
            SaveButton.IsEnabled = false;
            await Task.Run(() => BoardSerializer.Serialize(Board, dialog.FileName));
            SaveButton.IsEnabled = true;
            MessageBox.Show($"Game was saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.None);
        }
        catch(JsonSerializationException)
        {
            MessageBox.Show("Error saving game to a file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
    {
        ResetAutoSlider();

        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "JPEG image (.jpeg)|*.jpeg";
        dialog.FileName = "GOL_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg";

        if (dialog.ShowDialog() == false)
            return;

        Screenshot.SaveUIElementToJpeg(GameGrid, dialog.FileName);

        try
        {
            Process.Start("rundll32.exe", "shell32.dll,OpenAs_RunDLL " + dialog.FileName);
        }
        catch (Exception)
        {
            MessageBox.Show("Error saving a screenshot", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}
