namespace AdventOfCode;

internal static class ConsoleHelper
{
    internal static void WriteColored(string message, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        var previousColor = Console.ForegroundColor;
        var previousBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor ?? previousColor;
        Console.BackgroundColor = backgroundColor ?? previousBackgroundColor;
        Console.Write(message);
        Console.ForegroundColor = previousColor;
        Console.BackgroundColor = previousBackgroundColor;
    }
    
    internal static void WriteLineColored(string message, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        WriteColored(message, foregroundColor, backgroundColor);
        Console.WriteLine();
    }
    
    private static ConsoleColor TaskTimeColor(TimeSpan time) => time switch {
        { TotalMilliseconds: < Consts.TotalTaskYellowMilliseconds } => ConsoleColor.Green,
        { TotalMilliseconds: < Consts.TotalTaskRedMilliseconds } => ConsoleColor.Yellow,
        _ => ConsoleColor.Red
    };
    
    private static ConsoleColor WholeRunTimeColor(TimeSpan time) => time switch {
        { TotalMinutes: < Consts.TotalWholeRunYellowMilliseconds } => ConsoleColor.Green,
        { TotalMinutes: < Consts.TotalWholeRunRedMilliseconds } => ConsoleColor.Yellow,
        _ => ConsoleColor.Red
    };
    
    internal static void PrintTaskTime(TimeSpan time) => WriteColored(time.ToString(), TaskTimeColor(time));
    
    internal static void PrintWholeRunTime(TimeSpan time) => WriteColored(time.ToString(), WholeRunTimeColor(time));
}