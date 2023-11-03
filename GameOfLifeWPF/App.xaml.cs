using System.Windows;
using System.Windows.Controls;

namespace GameOfLifeWPF;

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
