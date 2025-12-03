using System;
using AdventOfCode.View.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AdventOfCode.View;

internal sealed class App : Application
{
    public required Lazy<MainWindow> MainWindowFactory { private get; init; }
        
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = MainWindowFactory.Value;

        base.OnFrameworkInitializationCompleted();
    }
}