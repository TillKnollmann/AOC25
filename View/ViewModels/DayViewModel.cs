using System;
using AdventOfCode.Days;

namespace AdventOfCode.View.ViewModels;

internal sealed class DayViewModel(
    IDay day,
    Func<IDay, FirstPartViewModel> firstPartFactory,
    Func<IDay, SecondPartViewModel> secondPartFactory) 
    : ViewModelBase
{
    internal int Year => day.Year;
    internal int Number => day.Number;
    internal int? SampleNumber => day.SampleNumber;
    internal string Input
    {
        get => day.Input;
        set
        {
            day.OverrideInput(value);
            OnPropertyChanged();
        }
    }

    internal FirstPartViewModel FirstPart { get; } = firstPartFactory(day);
    internal SecondPartViewModel SecondPart { get; } = secondPartFactory(day);
}