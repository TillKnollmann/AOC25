namespace AdventOfCode.Days.Year2025;

internal class Day02 : DayOfYear2025<Day02, List<Day02.Range>>
{
    public override int Number => 2;

    public override string FirstPart()
    {
        var ranges = ParsedInput.Value;
        return ranges
            .SelectMany(GetInvalidIdsPart1)
            .Sum()
            .ToString();
    }

    public override string SecondPart()
    {
        var ranges = ParsedInput.Value;
        return ranges
            .SelectMany(GetInvalidIdsPart2)
            .Sum()
            .ToString();
    }

    private static List<long> GetInvalidIdsPart1(Range range)
    {
        List<long> invalidIds = [];
        for (var i = range.Start; i <= range.End; i++)
            if (IsIdInvalidPart1(i))
                invalidIds.Add(i);
        return invalidIds;
    }

    private static bool IsIdInvalidPart1(long id)
    {
        var idString = id.ToString();
        if (idString.Length % 2 != 0)
            return false;
        var firstHalf = idString.Substring(0, idString.Length / 2);
        var secondHalf = idString.Substring(idString.Length / 2);
        return firstHalf.Equals(secondHalf);
    }

    private static List<long> GetInvalidIdsPart2(Range range)
    {
        List<long> invalidIds = [];
        for (var i = range.Start; i <= range.End; i++)
            if (IsIdInvalidPart2(i))
                invalidIds.Add(i);
        return invalidIds;
    }

    private static bool IsIdInvalidPart2(long id)
    {
        var idString = id.ToString();
        for (var i = 1; i < idString.Length; i++)
        {
            var pattern = idString.Substring(0, i);
            if (idString.Replace(pattern, "").Length == 0)
                return true;
        }

        return false;
    }

    protected override List<Range> ParseInput()
    {
        return Input.Trim()
            .Split(",")
            .Select(ParseRange)
            .ToList();
    }

    private static Range ParseRange(string rangeString)
    {
        var numbers = rangeString.Split("-")
            .Select(long.Parse)
            .ToArray();
        return new Range(numbers[0], numbers[1]);
    }

    internal class Range(long start, long end)
    {
        public long Start { get; } = start;
        public long End { get; } = end;
    }
}