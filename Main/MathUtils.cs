namespace AdventOfCode;

internal static class MathUtils
{
    internal static long GetDigitCount(long number) => (long) Math.Floor(Math.Log10(number) + 1);
}