using AdventOfCode.View.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AdventOfCode.View.Views;

internal sealed partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }
}