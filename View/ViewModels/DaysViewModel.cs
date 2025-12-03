using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Days;
using AdventOfCode.Days.Pickers;

namespace AdventOfCode.View.ViewModels;

internal sealed class DaysViewModel : ViewModelBase
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, ImmutableArray<DayViewModel>>> _map = [];
    
    public DaysViewModel(
        ImmutableArray<IDay> days,
        PickAllWithSamples picker,
        Func<IDay, DayViewModel> dayViewModelFactory)
    {
        foreach (IDay day in picker.PickDays(days))
        {
            var daysMap = _map.GetOrAdd(day.Year, _ => []);
            var immutableArray = daysMap.GetOrAdd(day.Number, _ => []);
            daysMap[day.Number] = immutableArray.Add(dayViewModelFactory(day));
        }
        
        SelectedYear = _map.Last().Key;
        SelectedDayInstance = Samples.Last();
    }

    internal ICollection<int> Years => _map.Keys;

    internal int SelectedYear
    {
        get;
        set
        {
            if (!SetField(ref field, value))
                return;
            _selectedDaysMap = _map[value];
            OnPropertyChanged(nameof(Days));
            SelectedDay = _selectedDaysMap.Last().Key;
        }
    }
    
    private ConcurrentDictionary<int, ImmutableArray<DayViewModel>> _selectedDaysMap = [];
    internal ICollection<int> Days => _selectedDaysMap.Keys;

    internal int SelectedDay
    {
        get;
        set
        {
            if (!SetField(ref field, value))
                return;
            Samples = _selectedDaysMap[value];
        }
    }
    
    internal ImmutableArray<DayViewModel> Samples
    {
        get;
        set 
        { 
            if (!SetField(ref field, value))
                return;
            SelectedDayInstance = value.Last();
        }
    }

    internal DayViewModel SelectedDayInstance
    {
        get;
        set => SetField(ref field, value);
    }
}