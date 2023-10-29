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
                new BoardPresetItem("Acorn", typeof(AcornBoardFactory), 7, 3),
                new BoardPresetItem("Glider", typeof(GliderBoardFactory), 3, 3),
                new BoardPresetItem("Light-weight Spaceship", typeof(LWSSBoardFactory), 5, 4),
                new BoardPresetItem("Middle-weight Spaceship", typeof(MWSSBoardFactory), 6, 5),
                new BoardPresetItem("Heavy-weight Spaceship", typeof(HWSSBoardFactory), 7, 5),
                new BoardPresetItem("Pulsar", typeof(PulsarBoardFactory), 13, 13),
                new BoardPresetItem("Glider Gun", typeof(GliderGunBoardFactory), 36, 9)
            };
            DataContext = this;
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
