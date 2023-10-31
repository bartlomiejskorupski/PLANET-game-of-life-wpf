using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameOfLifeWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();

            base.OnStartup(e);
        }

        public static void Navigate(UserControl navigateTo)
        {
            _mainWindow.MainContent.Content = navigateTo;
        }
    }
}
