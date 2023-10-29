using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.BoardFactory;
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
        public List<BoardPresetItem> BoardPresetItems { get; set; }
        public CreateGameView()
        {
            InitializeComponent();
            BoardPresetItems = new List<BoardPresetItem>()
            {
                new BoardPresetItem("Empty", typeof(EmptyBoardFactory)),
                new BoardPresetItem("Acorn (7x3)", typeof(AcornBoardFactory)),
                new BoardPresetItem("Glider (3x3)", typeof(GliderBoardFactory)),
                new BoardPresetItem("Light-weight Spaceship (5x4)", typeof(LWSSBoardFactory)),
                new BoardPresetItem("Middle-weight Spaceship (6x5)", typeof(MWSSBoardFactory)),
                new BoardPresetItem("Heavy-weight Spaceship (7x5)", typeof(HWSSBoardFactory)),
                new BoardPresetItem("Pulsar (13x13)", typeof(PulsarBoardFactory)),
                new BoardPresetItem("Glider Gun (36x9)", typeof(GliderGunBoardFactory))
            };
            BoardPresetListBox.ItemsSource = BoardPresetItems;
            BoardPresetListBox.SelectedIndex = 0;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(!Int32.TryParse(WidthTB.Text, out int width) || !Int32.TryParse(HeightTB.Text, out int height))
            {
                MessageBox.Show("Error parsing width and height", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BoardPresetItem preset = (BoardPresetItem)BoardPresetListBox.SelectedItem;
            IBoardFactory factory = (IBoardFactory)Activator.CreateInstance(preset.BoardFactoryType, new object[] {width, height});

            if (!factory.CanCreate())
            {
                MessageBox.Show("Cannot create board.\nWidth or height is too small.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.Navigate(this, new GameView(factory));
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
