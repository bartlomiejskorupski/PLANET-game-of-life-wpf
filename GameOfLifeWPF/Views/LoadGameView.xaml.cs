using GameOfLifeWPF.Model.BoardFactory;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace GameOfLifeWPF.Views;

public partial class LoadGameView : UserControl
{
    private string _chosenFilePath;

    public LoadGameView()
    {
        InitializeComponent();
        _chosenFilePath = "";
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        App.Navigate(new TitleView());
    }

    private void ChooseFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Game of life save file (.gol)|*.gol";

        if (dialog.ShowDialog() == false)
        {
            _chosenFilePath = "";
            ChosenFileLabel.Text = "No file chosen";
            ContinueButton.IsEnabled = false;
            return;
        }

        _chosenFilePath = dialog.FileName;
        var fileName = System.IO.Path.GetFileName(_chosenFilePath);

        ChosenFileLabel.Text = fileName;
        ContinueButton.IsEnabled = true;
        ContinueButton.Focus();
    }

    private void ContinueButton_Click(object sender, RoutedEventArgs e)
    {
        var factory = new LoadBoardFactory(_chosenFilePath);
        if (!factory.CanCreate())
        {
            MessageBox.Show($"Error loading file.\nThe file may be corrupted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        App.Navigate(new GameView(factory));
    }
}
