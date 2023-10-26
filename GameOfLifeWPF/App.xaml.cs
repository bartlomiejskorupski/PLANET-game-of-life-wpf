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
        public static void Navigate(UserControl caller, UserControl navigateTo)
        {
            var window = Window.GetWindow(caller);
            var mainContent = (ContentControl)window.FindName("MainContent");
            mainContent.Content = navigateTo;
        }
    }
}
