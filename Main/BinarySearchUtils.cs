namespace AdventOfCode;

internal static class BinarySearchUtils
{
    /// <summary>
    /// Returns first value where the predicate is false.
    /// </summary>
    internal static int ThresholdSearch(int start, int end, Func<int, bool> predicate)
    {
        if (!predicate(start)) 
            return start;
        if (predicate(end))
            return end + 1;
        while (start + 1 != end)
        {
            var mid = (start + end) / 2;
            var midResult = predicate(mid);
            if (midResult)
                start = mid;
            else
                end = mid;
        }
        return end;
    }
}