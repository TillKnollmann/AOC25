namespace AdventOfCode.Extensions;

internal static class IReadOnlyListExtensions
{
    internal static IEnumerable<(T, T)> AllPairs<T>(this IReadOnlyList<T> source)
    {
        var upperBound = source.Count - 1;
        for (var i = 0; i < upperBound; i++)
        {
            for (var j = i + 1; j < source.Count; j++)
            {
                yield return (source[i], source[j]);
            }
        }
    }
}