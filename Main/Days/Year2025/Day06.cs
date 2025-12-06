using System.Text.RegularExpressions;

namespace AdventOfCode.Days.Year2025;

internal class Day06 : DayOfYear2025<Day06, List<Day06.Calculation>>
{
    public override int Number => 6;

    protected override List<Calculation> ParseInput()
    {
        List<int> separatorLines = [];
        var inputLines = Input.Split(Environment.NewLine);
        for (var i = 0; i < inputLines[0].Length; i++)
        {
            if (inputLines.All(line => line.ElementAt(i).ToString().IsWhiteSpace()))
                separatorLines.Add(i);
        }

        List<Calculation> calculations = [];
        for (var i = 0; i < separatorLines.Count + 1; i++)
        {
            var start = 0;
            if (i > 0)
                start = separatorLines[i - 1] + 1;
            var end = inputLines[0].Length;
            if (i < separatorLines.Count)
                end = separatorLines[i];
            var lines = inputLines.Take(inputLines.Length - 1).Select(line => line[start..end]).ToList();
            var operation = inputLines.Last()[start..end].Trim() switch
            {
                "+" => Operation.Addition,
                "*" => Operation.Multiplication,
                _ => throw new ArgumentOutOfRangeException()
            };
            calculations.Add(new Calculation(lines, operation));
        }

        return calculations;
    }

    public override string FirstPart()
    {
        var calculations = ParseInput();
        return calculations.Select(calculation => calculation.Operation switch
        {
            Operation.Addition => calculation.Lines.Select(long.Parse).Sum(),
            Operation.Multiplication => calculation.Lines.Select(long.Parse).Aggregate(1L, (acc, val) => acc * val),
            _ => throw new ArgumentOutOfRangeException()
        }).Sum().ToString();
    }

    public override string SecondPart()
    {
        var calculations = ParseInput();
        return calculations.Select(calculation =>
            {
                var numberCount = calculation.Lines[0].Length;
                List<long> numbers = [];
                for (var i = numberCount - 1; i >= 0; i--)
                {
                    numbers.Add(long.Parse(string.Concat(calculation.Lines.Select(line => line[i]))));
                }

                return calculation.Operation switch
                {
                    Operation.Addition => numbers.Sum(),
                    Operation.Multiplication => numbers.Aggregate(1L, (acc, val) => acc * val),
                    _ => throw new ArgumentOutOfRangeException()
                };
            })
            .Sum()
            .ToString();
    }

    internal class Calculation(List<string> lines, Operation operation)
    {
        public List<string> Lines { get; } = lines;
        public Operation Operation { get; } = operation;
    }

    internal enum Operation
    {
        Addition,
        Multiplication
    }
}