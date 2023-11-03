using GameOfLifeWPF.Views;
using System.Windows;

namespace GameOfLifeWPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new TitleView();
    }
}
