using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
using GameOfLifeWPF.Model.ScreenCapture;
using GameOfLifeWPF.Model.Serialization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<RectangleViewModel> Cells { get; } = new ObservableCollection<RectangleViewModel>();


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

        private void UpdateCanvas()
        {
            CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

            Canvas canvas = FindVisualChild<Canvas>(GameCanvas);

            if (canvas != null)
            {
                foreach (ContentPresenter child in canvas.Children)
                {
                    if (child.Content is RectangleViewModel rectangleViewModel)
                    {
                        // Access the visual representation (Rectangle) within the DataTemplate
                        Rectangle rectangle = FindVisualChild<Rectangle>(child);

                        if (rectangle != null)
                        {
                            rectangle.MouseDown -= OnRectangle_MouseDown;
                        }
                    }
                }
            }

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
                Fill = new SolidColorBrush(Colors.Gray)
            });

            for (int x = 0; x < currentState.Width; x++)
            {
                for (int y = 0; y < currentState.Height; y++)
                {
                    var cellAlive = currentState.Cells[x, y].IsAlive;
                    if (cellAlive)
                        Cells.Add(new RectangleViewModel()
                        {
                            Width = cellSize,
                            Height = cellSize,
                            Left = cellSize * x - offsetLeft,
                            Top = cellSize * y - offsetTop,
                            X = x,
                            Y = y
                        });
                }
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

        private void OnRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = (Rectangle)sender;
            var rectVM = (RectangleViewModel)rect.DataContext;

            int cellX, cellY;

            if (rectVM.X != -1 && rectVM.Y != -1)
            {
                cellX = rectVM.X;
                cellY = rectVM.Y;

                Cells.Remove(rectVM);
            }
            else
            {
                CalculateCanvasDimensions(out double cellSize, out double offsetLeft, out double offsetTop);

                var clickPoint = e.GetPosition(rect);
                cellX = (int)(clickPoint.X / cellSize);
                cellY = (int)(clickPoint.Y / cellSize);

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

            Board.CurrentState.ToggleCellState(cellX, cellY);
            AliveTB.Text = Board.AliveText;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T found)
                {
                    return found;
                }

                T foundChild = FindVisualChild<T>(child);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            return null;
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCanvas();
        }

        private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
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

            Board = null;
            DataContext = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForFullGCComplete();

            App.Navigate(new TitleView());
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
