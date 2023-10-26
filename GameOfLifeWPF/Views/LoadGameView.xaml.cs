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
        public LoadGameView()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(this, new TitleView());
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
;
            if (dialog.ShowDialog() == false)
            {
                return;
            }
            MessageBox.Show($"{dialog.FileName}");
        }
    }
}
