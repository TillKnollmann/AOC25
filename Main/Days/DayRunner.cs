using System.Diagnostics;
using AdventOfCode.Days.Pickers;
using MrMeeseeks.DIE.UserUtility;

namespace AdventOfCode.Days;

internal interface IDayRunner
{
    void Run();
}

internal class DayRunner : IDayRunner, IScopeRoot
{
    private readonly IReadOnlyList<IDay> _days;
    private readonly IDayPicker _picker;
    private readonly IResultsPrinter _resultsPrinter;
    private readonly ITimesPrinter _timesPrinter;

    internal DayRunner(
        IReadOnlyList<IDay> days,
        IDayPicker picker,
        IResultsPrinter resultsPrinter,
        ITimesPrinter timesPrinter)
    {
        _days = days;
        _picker = picker;
        _resultsPrinter = resultsPrinter;
        _timesPrinter = timesPrinter;
    }

    public void Run()
    {
        var start = Stopwatch.GetTimestamp();
        foreach (var day in _picker.PickDays(_days))
        {
            PrintInput(day);
            day.PrepareInputParsing();
            day.FirstPart();
            day.SecondPart();
        }
        var end = Stopwatch.GetTimestamp();
        var time = Stopwatch.GetElapsedTime(start, end);
        Console.Write("Whole run took ");
        ConsoleHelper.PrintWholeRunTime(time);
        Console.WriteLine(".");
        Console.WriteLine();
        
        _resultsPrinter.Print();
        _timesPrinter.Print();

        return;
        
        void PrintInput(IDay day)
        {
            day.PrintLongDayLabel();
            Console.WriteLine(" Input:");
            var shortenedInput = day.Input.Select((c, i) => (c, i)).Where(t => t.c is '\n').Take(5).Skip(4).FirstOrDefault() 
                is (_, var index and > 0)
                ? (Shortened: true, Input: day.Input[..index])
                : (Shortened: false, Input: day.Input);
            foreach (var line in shortenedInput.Input.Split(Environment.NewLine))
            {
                if (line.Length > Console.WindowWidth)
                {
                    ConsoleHelper.WriteColored(line[..(Console.WindowWidth - 3)], ConsoleColor.DarkGray);
                    ConsoleHelper.WriteLineColored("...", ConsoleColor.DarkYellow);
                }
                else
                {
                    ConsoleHelper.WriteLineColored(line, ConsoleColor.DarkGray);
                }
            }

            if (shortenedInput.Shortened)
            {
                ConsoleHelper.WriteLineColored("...", ConsoleColor.DarkYellow);
            }

            Console.WriteLine();
        }
    }
}