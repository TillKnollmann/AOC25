using System.Collections.Immutable;
using MrMeeseeks.DIE.UserUtility;

namespace AdventOfCode.Days;

internal interface ITimesPrinter
{
    void Print();
}

internal interface ITimesRegistry
{
    void Register(IDay day, Stage stage, TimeSpan duration);
}

internal class TimesRegistry : ITimesRegistry, ITimesPrinter, IScopeInstance
{
    private readonly List<(IDay Day, Stage Stage, TimeSpan Duration)> _results = [];
    public void Register(IDay day, Stage stage, TimeSpan duration) => _results.Add((day, stage, duration));
    public void Print()
    {
        var maxLabelLength = _results.Select(t => t.Day.GetShortDayLabelText(Stage.SecondPart)).Max(x => x.Length);
        Console.WriteLine("Times Summary:");
        if (_results.Count > 0)
        {
            var lineDescriptionGroups = GetLineDescriptions(_results);
            
            while (lineDescriptionGroups.Count > 0)
            {
                var lineDescriptionGroup = lineDescriptionGroups.Pop();
                foreach (var lineDescription in lineDescriptionGroup)
                {
                    PrintLine(lineDescription);
                }

                if (lineDescriptionGroups.Count > 0)
                {
                    Console.WriteLine("<<<");
                }
            }
        }
        else
            Console.WriteLine("No times to display.");

        Console.WriteLine();
        return;

        Stack<ImmutableArray<LineDescription>> GetLineDescriptions(IReadOnlyList<(IDay Day, Stage Stage, TimeSpan Duration)> results)
        {
            Stack<ImmutableArray<LineDescription>> lineGroups = [];
            var currentResults = results;

            while (currentResults.Count > 0)
            {
                var timeSpanTextLength = new TimeSpan(0, 23, 59, 59, 999, 999).ToString().Length;
                var barLength = Console.WindowWidth - maxLabelLength - 2 /* delimiting spaces */ - timeSpanTextLength;
                var maxTime = currentResults.MaxBy(x => x.Duration).Duration;
            
                var timeUnit = maxTime.TotalMilliseconds / barLength;
                var greenUnitCount = (int) (Consts.TotalTaskYellowMilliseconds / timeUnit);
                var yellowUnitCount = (int) (Consts.TotalTaskRedMilliseconds / timeUnit) - greenUnitCount;
                var groups = currentResults
                    .Select(t =>
                    {
                        var (day, stage, duration) = t;
                        var dayLabelLength = day.GetShortDayLabelText(stage).Length;
                        var timeUnitCount = (int) (duration.TotalMilliseconds / timeUnit);
                        var greens = Math.Max(Math.Min(timeUnitCount, greenUnitCount), 1);
                        timeUnitCount -= greenUnitCount;
                        var yellows = timeUnitCount > 0 ? Math.Min(timeUnitCount, yellowUnitCount) : 0;
                        timeUnitCount -= yellowUnitCount;
                        var reds = Math.Max(timeUnitCount, 0);
                        var remainingSpaces = barLength - greens - yellows - reds;
                        return new LineDescription(day, stage, dayLabelLength, greens, yellows, reds, remainingSpaces, duration);
                    })
                    .GroupBy(x => x.Greens > 1);

                var currentLines = groups.First(g => g.Key).OrderBy(x => x.Duration).ToImmutableArray();
                lineGroups.Push(currentLines);
                if (groups.FirstOrDefault(x => !x.Key) is { } nextGroup)
                    currentResults = [..nextGroup.Select(x => (x.Day, x.Stage, x.Duration))];
                else
                    currentResults = [];
            }

            return lineGroups;
        }

        void PrintLine(LineDescription lineDescription)
        {
            var (day, stage, dayLabelLength, greens, yellows, reds, remainingSpaces, duration) = lineDescription;
            day.PrintShortDayLabel(stage);
            Console.Write($"{"".PadRight(maxLabelLength - dayLabelLength, ' ')} ");
            ConsoleHelper.WriteColored("".PadRight(greens, '|'), ConsoleColor.DarkGreen);
            ConsoleHelper.WriteColored("".PadRight(yellows, '|'), ConsoleColor.DarkYellow);
            ConsoleHelper.WriteColored("".PadRight(reds, '|'), ConsoleColor.DarkRed);
            Console.Write("".PadRight(remainingSpaces, ' '));
            Console.Write(" ");
            ConsoleHelper.PrintTaskTime(duration);
            Console.WriteLine();
        }
    }
    
    private record LineDescription(IDay Day, Stage Stage, int DayLabelLength, int Greens, int Yellows, int Reds, int RemainingSpaces, TimeSpan Duration);
}