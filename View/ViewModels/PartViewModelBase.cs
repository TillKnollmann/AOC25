using AdventOfCode.Days;

namespace AdventOfCode.View.ViewModels;

internal abstract class PartViewModelBase : ViewModelBase
{
    internal bool Solved
    { 
        get;
        private set => SetField(ref field, value);
    }

    internal string Solution
    {
        get;
        private set => SetField(ref field, value);
    } = "";

    protected abstract string Solve();
    
    internal void SolvePart()
    {
        Solution = Solve();
        Solved = true;
    }
}

internal sealed class FirstPartViewModel(IDay day) : PartViewModelBase
{
    protected override string Solve() => 
        day.FirstPart();
}

internal sealed class SecondPartViewModel(IDay day) : PartViewModelBase
{
    protected override string Solve() => 
        day.SecondPart();
}