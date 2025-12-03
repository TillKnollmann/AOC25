namespace AdventOfCode.View.ViewModels;

internal sealed class MainWindowViewModel(DaysViewModel daysViewModel) : ViewModelBase
{
    internal DaysViewModel Days { get; } = daysViewModel;
}