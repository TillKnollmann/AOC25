using System.Diagnostics;
using MrMeeseeks.DIE.UserUtility;

namespace AdventOfCode.Days;

internal class DayDecoratorTrackTime : IDay, IDecorator<IDay>
{
    private readonly IDay _inner;
    private readonly ITimesRegistry _timesRegistry;

    internal DayDecoratorTrackTime(IDay inner, ITimesRegistry timesRegistry)
    {
        _inner = inner;
        _timesRegistry = timesRegistry;
    }
    
    public int Year => _inner.Year;
    public int Number => _inner.Number;
    public int? SampleNumber => _inner.SampleNumber;
    public string Input => _inner.Input;
    public void PrepareInputParsing()
    {
        var start = Stopwatch.GetTimestamp();
        _inner.PrepareInputParsing();
        var end = Stopwatch.GetTimestamp();
        PrintTracking(Stage.Parsing, Stopwatch.GetElapsedTime(start, end));
    }

    public string FirstPart()
    {
        var start = Stopwatch.GetTimestamp();
        var result = _inner.FirstPart();
        var end = Stopwatch.GetTimestamp();
        PrintTracking(Stage.FirstPart, Stopwatch.GetElapsedTime(start, end));
        return result;
    }

    public string SecondPart()
    {
        var start = Stopwatch.GetTimestamp();
        var result = _inner.SecondPart();
        var end = Stopwatch.GetTimestamp();
        PrintTracking(Stage.SecondPart, Stopwatch.GetElapsedTime(start, end));
        return result;
    }

    public IEnumerable<IDay> Samples() => _inner.Samples();
    public void PrintLongDayLabel(Stage? stage = null) => _inner.PrintLongDayLabel(stage);
    public void PrintShortDayLabel(Stage stage) => _inner.PrintShortDayLabel(stage);
    public string GetShortDayLabelText(Stage stage) => _inner.GetShortDayLabelText(stage);
    public void OverrideInput(string newInput) => _inner.OverrideInput(newInput);

    private void PrintTracking(Stage stage, TimeSpan time)
    {
        _inner.PrintLongDayLabel(stage);
        Console.Write(" Solution took ");
        ConsoleHelper.PrintTaskTime(time);
        Console.WriteLine(".");
        Console.WriteLine();

        _timesRegistry.Register(this, stage, time);
    }
}