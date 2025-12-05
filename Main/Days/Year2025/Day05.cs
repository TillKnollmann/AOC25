namespace AdventOfCode.Days.Year2025;

internal class Day05 : DayOfYear2025<Day05, (List<Tuple<long, long>>, List<long>)>
{
    public override int Number => 5;

    protected override (List<Tuple<long, long>>, List<long>) ParseInput()
    {
        var parts = Input.Split(Environment.NewLine + Environment.NewLine);
        var intervals = parts[0]
            .Split(Environment.NewLine)
            .Select(x => x.Split("-"))
            .Select(y => new Tuple<long, long>(long.Parse(y[0].Trim()), long.Parse(y[1].Trim())))
            .ToList();
        var numbers = parts[1]
            .Split(Environment.NewLine)
            .Select(long.Parse)
            .ToList();
        return (intervals, numbers);
    }

    public override string FirstPart()
    {
        var (intervals, numbers) = ParsedInput.Value;
        intervals = MergeIntervals(intervals);
        var count = 0;
        foreach (var number in numbers)
        foreach (var interval in intervals)
            if (number >= interval.Item1 && number <= interval.Item2)
            {
                count++;
                break;
            }

        return count.ToString();
    }

    public override string SecondPart()
    {
        var (intervals, _) = ParsedInput.Value;
        intervals = MergeIntervals(intervals);
        return intervals
            .Select((interval, _) => interval.Item2 - interval.Item1 + 1)
            .Sum()
            .ToString();
    }

    private static List<Tuple<long, long>> MergeIntervals(List<Tuple<long, long>> intervals)
    {
        intervals.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        var merged = new List<Tuple<long, long>>();
        long currentStart = -1;
        long currentEnd = -1;
        foreach (var ((start, end), index) in intervals.Select((value, index) => (value, index)))
        {
            if (start > currentEnd)
            {
                if (currentStart != -1)
                    merged.Add(new Tuple<long, long>(currentStart, currentEnd));
                currentStart = start;
                currentEnd = end;
            }
            else
            {
                currentEnd = Math.Max(end, currentEnd);
            }
            if (index == intervals.Count - 1) merged.Add(new Tuple<long, long>(currentStart, currentEnd));
        }
        return merged;
    }
}