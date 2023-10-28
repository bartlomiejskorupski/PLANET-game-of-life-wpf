using GameOfLifeWPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace GameOfLifeWPF.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public Board Board { get; set; }

        public GameView(IBoardFactory boardFactory)
        {
            InitializeComponent();
            Board = boardFactory.CreateBoard();
            DataContext = Board;
        }

        private void UpdateCanvas()
        {
            GameCanvas.Children.Clear();

            double cellWidth = GameCanvas.ActualWidth / Board.Width;
            double cellHeight = GameCanvas.ActualHeight / Board.Height;

            var currentState = Board.CurrentState;
            for(int x = 0; x < currentState.Width; x++)
            {
                for(int y = 0; y < currentState.Height; y++)
                {
                    var cellState = currentState.Cells[x, y].IsAlive;
                    AddCellRectangle(x, y, cellWidth, cellHeight, cellState);
                }
            }
        }

        private void AddCellRectangle(int x, int y, double width, double height, bool cellState)
        {
            var rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.StrokeThickness = 1;
            rect.Fill = new SolidColorBrush(cellState ? Colors.White : Colors.Gray);
            rect.DataContext = new Point(x, y);
            rect.MouseDown += OnCellMouseDown;
            Canvas.SetLeft(rect, x * width);
            Canvas.SetTop(rect, y * height);
            GameCanvas.Children.Add(rect);
        }

        private void OnCellMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            var cellPos = (Point)rect.DataContext;
            var x = (int)cellPos.X;
            var y = (int)cellPos.Y;

            Board.CurrentState.ToggleCellState(x, y);
            var cell = Board.CurrentState.Cells[x, y];
            AliveTB.Text = Board.AliveText;

            rect.Fill = new SolidColorBrush(cell.IsAlive ? Colors.White : Colors.Gray);
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
            UpdateCanvas();
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
    }
}
