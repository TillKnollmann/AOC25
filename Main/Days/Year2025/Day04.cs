namespace AdventOfCode.Days.Year2025;

internal class Day04 : DayOfYear2025<Day04, Day04.Grid<char>>
{

    public override int Number => 4;
    
    private const char Empty = '.';
    private const char PaperRoll = '@';
    
    protected override Grid<char> ParseInput()
    {
        return new Grid<char>(Input.Trim()
            .Split("\n")
            .Select(row => row.Trim().ToCharArray())
            .ToArray());
    }

    public override string FirstPart()
    {
        var grid = ParsedInput.Value;
        var count = 0;
        
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                if (CanBeRemoved(x, y, grid))
                {
                    count++;
                }
            }
        }

        return count.ToString();
    }

    public override string SecondPart()
    {
        var grid = ParsedInput.Value;
        var difference = true;
        var count = 0;

        while (difference)
        {
            difference = false;
            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    if (CanBeRemoved(x, y, grid))
                    {
                        count++;
                        grid = RemovePaperRoll(x, y, grid);
                        difference = true;
                    }
                }
            }
        }

        return count.ToString();
    }

    private static Grid<char> RemovePaperRoll(int x, int y, Grid<char> grid)
    {
        var newData = grid.Data
            .Select(row => row.ToArray())
            .ToArray();
        
        newData[y][x] = Empty;
        return new Grid<char>(newData);
    }
    
    private static bool CanBeRemoved(int x, int y, Grid<char> grid)
    {
        if (!IsPaperRoll(x, y, grid))
            return false;

        var adjacentPaperRollsCount = GetAdjacentPaperRollsCount(x, y, grid);
        return adjacentPaperRollsCount < 4;
    }
    
    private static int GetAdjacentPaperRollsCount(int x, int y, Grid<char> grid)
    {
        return GetAdjacentPositions(x, y, grid)
            .Count(position => IsPaperRoll(position.Item1, position.Item2, grid));
    }
    
    private static bool IsPaperRoll(int x, int y, Grid<char> grid)
    {
        return grid.Data[y][x] == PaperRoll;
    }
    
    private static List<Tuple<int, int>> GetAdjacentPositions(int x, int y, Grid<char> grid)
    {
        List<Tuple<int,int>> adjacentPositions = [];
        
        for (var currentX = x  -1; currentX <= x + 1; currentX++)
        {
            for (var currentY = y - 1; currentY <= y + 1; currentY++)
            {
                if (currentX == x && currentY == y)
                    continue;
                
                if (currentX < 0 || currentX >= grid.Width || currentY < 0 || currentY >= grid.Height)
                    continue;

                adjacentPositions.Add(new Tuple<int, int>(currentX, currentY));
            }
        }
        
        return adjacentPositions;
    }
    
    internal class Grid<T>(T[][] data)
    {
        public T[][] Data { get; } = data;
        public int Width => Data[0].GetLength(0);
        public int Height => Data.GetLength(0);
    }
}