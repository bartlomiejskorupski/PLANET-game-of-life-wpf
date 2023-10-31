using GameOfLifeWPF.Model.BoardFactory;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoadGameView.xaml
    /// </summary>
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
}
