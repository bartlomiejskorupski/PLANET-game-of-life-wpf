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
            MessageBox.Show($"Width: {WidthTB.Text} Height: {HeightTB.Text}", "Result");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(this, new TitleView());
        }
    }
}
