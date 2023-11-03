using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameOfLifeWPF.Views;

public partial class CreateGameView : UserControl
{
    public List<BoardPresetItem> BoardPresetItems { get; set; }
    public int MaxBoardSize { get; set; }
    public CreateGameView()
    {
        InitializeComponent();
        BoardPresetItems = new List<BoardPresetItem>()
        {
            new BoardPresetItem("Empty", typeof(EmptyBoardFactory)),
            new BoardPresetItem("R-Pentomino", typeof(RPentominoBoardFactory), 3, 3, "DarkGreen"),
            new BoardPresetItem("Acorn", typeof(AcornBoardFactory), 7, 3, "SaddleBrown"),
            new BoardPresetItem("Glider", typeof(GliderBoardFactory), 3, 3, "MidnightBlue"),
            new BoardPresetItem("Light-weight Spaceship", typeof(LWSSBoardFactory), 5, 4, "DarkGray"),
            new BoardPresetItem("Middle-weight Spaceship", typeof(MWSSBoardFactory), 6, 5, "Gray"),
            new BoardPresetItem("Heavy-weight Spaceship", typeof(HWSSBoardFactory), 7, 5, "DimGray"),
            new BoardPresetItem("Pulsar", typeof(PulsarBoardFactory), 13, 13, "DarkMagenta"),
            new BoardPresetItem("Glider Gun", typeof(GliderGunBoardFactory), 36, 9, "Maroon"),
            new BoardPresetItem("Bi-Block Puffer", typeof(BiBlockPufferBoardFactory), 37, 29, "Black"),
        };
        MaxBoardSize = 999;
        DataContext = this;
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        if(!Int32.TryParse(WidthTB.Text, out int width) || !Int32.TryParse(HeightTB.Text, out int height))
        {
            MessageBox.Show("Error parsing width and height", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if(width <= 0 || width > MaxBoardSize || height <= 0 || height > MaxBoardSize)
        {
            MessageBox.Show("Incorrect board size. Width and height\nmust be between 1 and 99 in size.\nRecommended size: 80x40", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        BoardPresetItem preset = (BoardPresetItem)BoardPresetListBox.SelectedItem;
        IBoardFactory factory = (IBoardFactory)Activator.CreateInstance(preset.BoardFactoryType, new object[] {width, height});

        if (!factory.CanCreate())
        {
            MessageBox.Show("Cannot create board.\nWidth or height is too small.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        App.Navigate(new GameView(factory));
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        App.Navigate(new TitleView());
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        var containsNonNumbers = regex.IsMatch(e.Text);
        e.Handled = containsNonNumbers;
    }
}
