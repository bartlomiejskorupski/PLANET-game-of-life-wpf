using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
using GameOfLifeWPF.Model.ScreenCapture;
using GameOfLifeWPF.Model.Serialization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLifeWPF.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public Board Board { get; set; }
        private DispatcherTimer _autoTimer;
        private DispatcherTimer _resizeTimer;

        public GameView(IBoardFactory boardFactory)
        {
            InitializeComponent();
            Board = boardFactory.CreateBoard();
            DataContext = Board;
            _autoTimer = new DispatcherTimer();
            _autoTimer.Tick += AutoTimerTick;
            _autoTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Tick += ResizeTimerTick;
            _resizeTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void CreateCanvas()
        {
            GameCanvas.Children.Clear();
            GameCanvas.Background = new SolidColorBrush(Colors.Gray);

            CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

            var currentState = Board.CurrentState;
            for (int x = 0; x < currentState.Width; x++)
            {
                for (int y = 0; y < currentState.Height; y++)
                {
                    var cellState = currentState.Cells[x, y].IsAlive;
                    AddCellRectangle(x, y, cellSize, cellState, offsetLeft, offsetTop);
                }
            }
        }

        private void AddCellRectangle(int x, int y, double cellSize, bool cellState, double offsetLeft, double offsetTop)
        {
            var rect = new Rectangle()
            {
                Width = cellSize,
                Height = cellSize,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 0.5,
                Fill = new SolidColorBrush(cellState ? Colors.White : Colors.Gray)
            };
            rect.DataContext = new RectCellData(x, y, cellState);
            rect.MouseDown += OnCellMouseDown;
            Canvas.SetLeft(rect, x * cellSize - offsetLeft);
            Canvas.SetTop(rect, y * cellSize - offsetTop);
            GameCanvas.Children.Add(rect);
        }

        private void UpdateCanvas()
        {
            CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

            foreach (Rectangle rect in GameCanvas.Children)
            {
                UpdateCellRect(rect, cellSize, offsetLeft, offsetTop);
            }
        }
        private void UpdateCellRect(Rectangle rect, double cellSize, double offsetLeft, double offsetTop)
        {
            var rectCellData = (RectCellData)rect.DataContext;

            if (rect.Width != cellSize)
            {
                rect.Width = cellSize;
                rect.Height = cellSize;
                Canvas.SetLeft(rect, rectCellData.X * cellSize - offsetLeft);
                Canvas.SetTop(rect, rectCellData.Y * cellSize - offsetTop);
            }
            
            var cell = Board.CurrentState.Cells[rectCellData.X, rectCellData.Y];

            if (cell.IsAlive == rectCellData.IsAlive)
                return;

            rectCellData.IsAlive = cell.IsAlive;
            rect.Fill = new SolidColorBrush(cell.IsAlive ? Colors.White : Colors.Gray);
        }

        private void CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop)
        {
            double contWidth = CanvasContainer.ActualWidth;
            double contHeight = CanvasContainer.ActualHeight;

            double containerRatio = contWidth / contHeight;
            double boardRatio = Board.Width / Board.Height;

            cellSize = 0.0;
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

        private void OnCellMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            var rectCellData = (RectCellData)rect.DataContext;
            var x = rectCellData.X;
            var y = rectCellData.Y;

            Board.CurrentState.ToggleCellState(x, y);
            rectCellData.IsAlive = !rectCellData.IsAlive;

            var cell = Board.CurrentState.Cells[x, y];
            AliveTB.Text = Board.AliveText;

            rect.Fill = new SolidColorBrush(cell.IsAlive ? Colors.White : Colors.Gray);
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            CreateCanvas();
        }

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeTimer.Start();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Board.NextGeneration();
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
            App.Navigate(this, new TitleView());
        }

        private void AutoToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            _autoTimer.Start();
        }

        private void AutoToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _autoTimer.Stop();
        }

        private void AutoTimerTick(object? sender, EventArgs e)
        {
            Board.NextGeneration();
            UpdateTopPanel();
            UpdateCanvas();
        }

        private void ResizeTimerTick(object? sender, EventArgs e)
        {
            _resizeTimer.Stop();
            UpdateCanvas();
        }

        private void GridButton_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach(Rectangle rect in GameCanvas.Children)
            {
                rect.StrokeThickness = 0.0;
            }
        }

        private void GridButton_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Rectangle rect in GameCanvas.Children)
            {
                rect.StrokeThickness = 1.0;
            }
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
}
