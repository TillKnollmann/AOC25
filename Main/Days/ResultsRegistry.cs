using System.Collections.Immutable;
using MrMeeseeks.DIE.UserUtility;

namespace AdventOfCode.Days;

internal interface IResultsPrinter
{
    void Print();
}

internal interface IResultsRegistry
{
    void Register(IDay day, Stage stage, ResultStatus status);
}

internal class ResultsRegistry : IResultsRegistry, IResultsPrinter, IScopeInstance
{
    private readonly Dictionary<ResultStatus, List<(IDay Day, Stage Stage)>> _results = new()
    {
        [ResultStatus.Incorrect] = [],
        [ResultStatus.Uncertain] = [],
        [ResultStatus.Correct] = []
    };
    public void Register(IDay day, Stage stage, ResultStatus status) => _results[status].Add((day, stage));
    public void Print()
    {
        Console.WriteLine("Results Summary:");
        foreach (var (resultStatus, tasks) in _results.OrderBy(x => x.Key))
        {
            ConsoleHelper.WriteLineColored(resultStatus.ToChar().ToString(), resultStatus.ToConsoleColor());
            if (tasks.Count == 0)
                Console.WriteLine("None");
            else
            {
                var labelSize = tasks.Select(x => x.Day.GetShortDayLabelText(Stage.SecondPart).Length).Max();
                var chunkSize = Console.WindowWidth / (labelSize + 2/* Comma & space */);
                var chunks = new Queue<(IDay Day, Stage Stage)[]>(tasks.Chunk(chunkSize));
                while (chunks.Count > 0)
                {
                    var chunk = chunks.Dequeue();
                    foreach (var (day, stage) in chunk)
                    {
                        var padLeftSize = labelSize - day.GetShortDayLabelText(stage).Length;
                        day.PrintShortDayLabel(stage);
                        Console.Write("".PadLeft(padLeftSize, ' '));
                        if (chunks.Count > 0 || chunk.Last().Day != day)
                            Console.Write(", ");
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.WriteLine();
    }
}