namespace AdventOfCode.Days;

internal static class IntExtensions
{
    internal static string TwoDigits(this int value) => value.ToString().PadLeft(2, '0');
}