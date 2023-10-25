﻿using System;
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
    /// Interaction logic for TitleView.xaml
    /// </summary>
    public partial class TitleView : UserControl
    {
        public TitleView()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            var mainContent = (ContentControl)window.FindName("MainContent");
            mainContent.Content = new GameView();
        }
    }
}
