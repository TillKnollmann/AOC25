namespace AdventOfCode.Days;

internal abstract class DayOfYear2025<TDay, TData> : DayBase<TDay, TData>, IDay where TDay : class, IDay
{
    public override int Year => 2025;
}

internal abstract class DayBase<TDay, TData> : IDay where TDay : class, IDay
{
    private protected Lazy<TData> ParsedInput;
    private string? _inputOverride;
    
    protected DayBase() => ParsedInput = new Lazy<TData>(ParseInput);
    
    public abstract int Year { get; }

    public abstract int Number { get; }

    public string Input
    {
        get
        {
            if (_inputOverride is not null)
                return _inputOverride;
            var yearText = Year.ToString();
            var numberText = Number.TwoDigits();
            var sampleNumberText = SampleNumber is 0 or null
                ? string.Empty
                : $".{SampleNumber.Value.TwoDigits()}";
            return Inputs.ResourceManager.GetString($"{yearText}.{numberText}{sampleNumberText}") ?? string.Empty;
        }
    }

    public required int? SampleNumber { get; init; }
    
    internal required Func<int?, TDay> SampleFactory { get; init; }
    
    internal required Func<IDay, DayDecoratorTrackTime> TrackTimeWrappingFactory { get; init; }
    
    internal required Func<IDay, DayDecoratorPrintSolution> PrintSolutionWrappingFactory { get; init; }

    public void PrepareInputParsing() => _ = ParsedInput.Value;
    protected abstract TData ParseInput();
    public abstract string FirstPart();
    public abstract string SecondPart();
    public IEnumerable<IDay> Samples()
    {
        var i = 0;
        while (Inputs.ResourceManager.GetString($"{Year.ToString()}.{Number.TwoDigits()}.{(i + 1).TwoDigits()}") is not null)
        {
            yield return PrintSolutionWrappingFactory(TrackTimeWrappingFactory(SampleFactory(++i)));
        }
    }

    public void PrintLongDayLabel(Stage? stage)
    {
        ConsoleHelper.WriteColored($"Day {Number.TwoDigits()} ({Year.ToString()})", ConsoleColor.DarkBlue);
        if (SampleNumber is > 0)
        {
            Console.Write(" ");
            ConsoleHelper.WriteColored($"Sample {SampleNumber?.TwoDigits()}", ConsoleColor.DarkYellow);
        }
        if (stage is null) 
            return;
        var definitiveStage = stage.Value;
        Console.Write(" ");
        ConsoleHelper.WriteColored(definitiveStage.ToLongLabel(), definitiveStage.ToColor());
    }

    public void PrintShortDayLabel(Stage stage)
    {
        Console.Write($"{Year.ToString()}.");
        ConsoleHelper.WriteColored(Number.TwoDigits(), ConsoleColor.DarkBlue);
        if (SampleNumber is > 0)
        {
            Console.Write(".");
            ConsoleHelper.WriteColored(SampleNumber?.TwoDigits() ?? "", ConsoleColor.DarkYellow);
        }
        ConsoleHelper.WriteColored(stage.ToShortLabel(), stage.ToColor());
    }

    public string GetShortDayLabelText(Stage stage)
    {
        var samplePart = SampleNumber is > 0 ? $".{SampleNumber?.TwoDigits() ?? ""}" : "";
        return $"{Year.ToString()}.{Number.TwoDigits()}{samplePart}{stage.ToShortLabel()}";
    }

    public void OverrideInput(string newInput)
    {
        _inputOverride = newInput;
        ParsedInput = new Lazy<TData>(ParseInput);
    }
}