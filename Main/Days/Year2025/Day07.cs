namespace AdventOfCode.Days.Year2025;

internal class Day07 : DayOfYear2025<Day07, char[][]>
{
    public override int Number => 7;

    protected override char[][] ParseInput()
    {
        return Input.Split(Environment.NewLine)
            .Select(line => line.ToCharArray())
            .ToArray();
    }

    public override string FirstPart()
    {
        var grid = ParseInput();
        var (startRow, startCol) = GetStartPosition(grid);
        var splitterPositions = SimulateAllBeams(startRow, startCol, grid).Item1;

        return splitterPositions.Count.ToString();
    }

    public override string SecondPart()
    {
        var grid = ParseInput();
        var (startRow, startCol) = GetStartPosition(grid);
        var (splitterPositions, endReachedPositions) = SimulateAllBeams(startRow, startCol, grid);

        // Strategy: Dynamic Programming
        // For each splitter, count how many ways it can be reached from the start
        // A splitter at (x, y) can be reached from any splitter with smaller x and y +/- 1
        // unless there is another splitter in between that "consumes" the beam

        var sortedTimelineNodes = splitterPositions.Concat(endReachedPositions)
            .Distinct()
            .OrderBy(tuple => tuple.Item1)
            .ThenBy(tuple => tuple.Item2)
            .ToList();

        Dictionary<(int, int), long>
            reachingPastsByPosition =
                new() { { sortedTimelineNodes.First(), 1 } }; // first splitter can only be reached by start
        foreach (var (x, y) in sortedTimelineNodes.Skip(1)) // exclude first splitter
        {
            var numberOfReachablePasts = reachingPastsByPosition
                .Where(entry => entry.Key.Item1 < x && Math.Abs(entry.Key.Item2 - y) == 1) // find descendants
                .Where(entry => !reachingPastsByPosition.Any(other =>
                    entry.Key.Item1 < other.Key.Item1
                    && other.Key.Item1 < x
                    && other.Key.Item2 == y)) // take only those not consumed by earlier splitter in same y
                .Sum(entry => entry.Value);
            reachingPastsByPosition.Add((x, y), numberOfReachablePasts);
        }

        return reachingPastsByPosition.Where(entry => entry.Key.Item1 == grid.Length - 1)
            .Sum(entry => entry.Value)
            .ToString();
    }

    private static (HashSet<(int, int)>, HashSet<(int, int)>) SimulateAllBeams(int startRow, int startCol,
        char[][] grid)
    {
        HashSet<(int, int)> beamEndPositions =
        [
            (startRow, startCol)
        ];
        HashSet<(int, int)> splitterPositions = [];
        HashSet<(int, int)> endReachedPositions = [];
        while (beamEndPositions.Count > 0)
        {
            HashSet<(int, int)> newBeamEndPositions = [];
            foreach (var beamEndPosition in beamEndPositions)
            {
                var ((hitX, hitY), eventType) = SimulateBeam(grid, beamEndPosition.Item1, beamEndPosition.Item2);
                if (eventType == Event.EndReached)
                {
                    endReachedPositions.Add((hitX, hitY));
                    continue;
                }

                if (!splitterPositions.Add((hitX, hitY))) continue; // already processed this splitter
                if (hitY - 1 >= 0)
                    newBeamEndPositions.Add((hitX, hitY - 1));
                if (hitY + 1 < grid[0].Length)
                    newBeamEndPositions.Add((hitX, hitY + 1));
            }

            beamEndPositions = newBeamEndPositions;
        }

        return (splitterPositions, endReachedPositions);
    }

    private static ((int, int), Event) SimulateBeam(char[][] grid, int startX, int startY)
    {
        var eventX = grid
            .Select((row, index) => (row, index))
            .FirstOrDefault(item => item.index > startX && IsSplitter(item.row[startY]),
                grid.Select((row, index) => (row, index)).Last())
            .index;
        var eventType = IsSplitter(grid[eventX][startY]) ? Event.SplitterHit : Event.EndReached;
        return ((eventX, startY), eventType);
    }


    private static bool IsSplitter(char c)
    {
        return c == '^';
    }

    private static (int, int) GetStartPosition(char[][] grid)
    {
        return grid.SelectMany((row, r) => row.Select((cell, c) => (cell, r, c)))
            .First(cell => cell.cell == 'S') is var startCell
            ? (startCell.r, startCell.c)
            : throw new InvalidOperationException("Start position 'S' not found in the grid.");
    }

    private enum Event
    {
        SplitterHit,
        EndReached
    }
}