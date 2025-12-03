namespace AdventOfCode.Extensions;

internal static class EnumerableExtensions
{
    internal static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> enumerable, T defaultValue)
    {
        var hasElements = false;
        foreach (var element in enumerable)
        {
            yield return element;
            hasElements = true;
        }
        if (!hasElements)
        {
            yield return defaultValue;
        }
    }
}