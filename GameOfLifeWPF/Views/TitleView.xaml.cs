using System.Windows;
using System.Windows.Controls;

namespace GameOfLifeWPF.Views;

public partial class TitleView : UserControl
{
    public TitleView()
    {
        InitializeComponent();
    }

    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
        App.Navigate(new CreateGameView());
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to exit?",
            "Exit", 
            MessageBoxButton.YesNo, 
            MessageBoxImage.Question
        );
        if(result == MessageBoxResult.Yes)
            Window.GetWindow(this).Close();
    }

    private void LoadGameButton_Click(object sender, RoutedEventArgs e)
    {
        App.Navigate(new LoadGameView());
    }
}
