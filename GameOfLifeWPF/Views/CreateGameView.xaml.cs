using GameOfLifeWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreateGameView.xaml
    /// </summary>
    public partial class CreateGameView : UserControl
    {
        public CreateGameView()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(!Int32.TryParse(WidthTB.Text, out int width) || !Int32.TryParse(HeightTB.Text, out int height))
            {
                MessageBox.Show("Error parsing width and height", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.Navigate(this, new GameView(new NewBoardFactory(width, height)));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(this, new TitleView());
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            var containsNonNumbers = regex.IsMatch(e.Text);
            e.Handled = containsNonNumbers;
        }
    }
}
