namespace AdventOfCode.Days;

internal static class DayHelper
{
    internal static void WriteDayLabel(IDay day, string partLabel)
    {
        Console.Write("Day ");
        Console.Write($"{day.Year.ToString()}.");
        ConsoleHelper.WriteColored(day.Number.ToString().PadLeft(2, '0'), ConsoleColor.DarkBlue);
        if (day.SampleNumber is > 0)
            ConsoleHelper.WriteColored($".{day.SampleNumber}", ConsoleColor.DarkYellow);
        if (!string.IsNullOrWhiteSpace(partLabel))
        {
            Console.Write(" ");
            ConsoleHelper.WriteColored(partLabel, ConsoleColor.Magenta);
            Console.Write(" Part");
        }
    }
}