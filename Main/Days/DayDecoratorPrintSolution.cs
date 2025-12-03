using MrMeeseeks.DIE.UserUtility;

namespace AdventOfCode.Days;

internal class DayDecoratorPrintSolution : IDay, IDecorator<IDay>
{
    private readonly IDay _inner;
    private readonly IResultsRegistry _resultsRegistry;

    internal DayDecoratorPrintSolution(IDay inner, IResultsRegistry resultsRegistry)
    {
        _inner = inner;
        _resultsRegistry = resultsRegistry;
    }

    public int Year => _inner.Year;
    public int Number => _inner.Number;
    public int? SampleNumber => _inner.SampleNumber;
    public string Input => _inner.Input;
    public void PrepareInputParsing() => _inner.PrepareInputParsing();

    public string FirstPart()
    {
        var result = _inner.FirstPart();
        PrintSolution(result, Stage.FirstPart);
        return result;
    }

    public string SecondPart()
    {
        var result = _inner.SecondPart();
        PrintSolution(result, Stage.SecondPart);
        return result;
    }

    public IEnumerable<IDay> Samples() => _inner.Samples();
    public void PrintLongDayLabel(Stage? stage = null) => _inner.PrintLongDayLabel(stage);
    public void PrintShortDayLabel(Stage stage) => _inner.PrintShortDayLabel(stage);
    public string GetShortDayLabelText(Stage stage) => _inner.GetShortDayLabelText(stage);
    public void OverrideInput(string newInput) => _inner.OverrideInput(newInput);

    private void PrintSolution(string solution, Stage stage)
    {
        var samplePart = SampleNumber is > 0 ? $".{(SampleNumber ?? 0).TwoDigits()}" : string.Empty;
        var taskLabel = $"{Year.ToString()}.{Number.TwoDigits()}{samplePart}{stage.ToShortLabel()}";
        var knownSolution = Solutions.ResourceManager.GetString(taskLabel);
        var result = knownSolution is null 
            ? ResultStatus.Uncertain
            : solution == knownSolution ? ResultStatus.Correct : ResultStatus.Incorrect;
        _resultsRegistry.Register(this, stage, result);
        _inner.PrintLongDayLabel(stage);
        Console.Write(" Solution: ");
        ConsoleHelper.WriteColored(result.ToChar().ToString(), result.ToConsoleColor());
        if (result == ResultStatus.Incorrect)
        {
            Console.Write(" (");
            ConsoleHelper.WriteColored(knownSolution ?? "", ConsoleColor.DarkGreen);
            Console.Write(")");
        }
        Console.WriteLine();
        var foregroundColor = solution is Consts.NoSolutionFound or Consts.NothingToDoHere || result == ResultStatus.Incorrect
            ? ConsoleColor.DarkRed 
            : result == ResultStatus.Uncertain 
                ? ConsoleColor.DarkYellow 
                : ConsoleColor.DarkGreen;
        ConsoleHelper.WriteLineColored(solution, foregroundColor);
        Console.WriteLine();
    }
}