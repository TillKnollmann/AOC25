namespace AdventOfCode.Days.Year2025;

internal class Day12 : DayOfYear2025<Day12, (List<Day12.Shape>, List<Day12.RegionRequest>)>
{
    public override int Number => 12;

    protected override (List<Shape>, List<RegionRequest>) ParseInput()
    {
        var blocks = Input.Split(Environment.NewLine + Environment.NewLine);
        var shapes = blocks.Take(blocks.Length - 1)
            .Select(block =>
                new Shape(block.Split(Environment.NewLine)
                    .Skip(1)
                    .ToList()))
            .ToList();
        var regionRequests = blocks[^1]
            .Split(Environment.NewLine)
            .Select(line =>
            {
                var parts = line.Split(":");
                var size = parts[0].Trim().Split("x");
                var width = long.Parse(size[0].Trim());
                var height = long.Parse(size[1].Trim());
                var shapeCounts = parts[1].Trim()
                    .Split(" ")
                    .Select(long.Parse)
                    .ToList();
                return new RegionRequest(width, height, shapeCounts);
            })
            .ToList();
        return (shapes, regionRequests);
    }

    public override string FirstPart()
    {
        // Strategy: Give an approximation by simply ruling out all region requests that cannot be fulfilled,
        // as there are not enough tiles to fit the required tiles.
        // -> We simply ignore that shapes may not fit perfectly into each other. 
        var (shapes, regionRequests) = ParsedInput.Value;
        var requestsThatMightFit = regionRequests.Where(regionRequest => MightFit(regionRequest, shapes))
            .ToList();
        var nonFittingRequests = regionRequests.Count - requestsThatMightFit.Count;
        var uncertainRequests = requestsThatMightFit.Where(regionRequest => MightNotFit(regionRequest, shapes))
            .ToList();
        var fittingRequests = regionRequests.Count - nonFittingRequests - uncertainRequests.Count;
        return (fittingRequests + uncertainRequests.Count).ToString();
    }

    public override string SecondPart()
    {
        return "";
    }

    private static bool MightFit(RegionRequest regionRequest, List<Shape> shapes)
    {
        var minRequiredSpace = shapes.Zip(regionRequest.ShapeCounts)
            .Select((tuple, _) => tuple.First.RequiredSpace * tuple.Second)
            .Sum();
        var maxAvailableSpace = regionRequest.Width * regionRequest.Height;
        return minRequiredSpace <= maxAvailableSpace;
    }

    private static bool MightNotFit(RegionRequest regionRequest, List<Shape> shapes)
    {
        var maxRequiredSpace = shapes.Zip(regionRequest.ShapeCounts)
            .Select((tuple, _) => tuple.First.Definition.Count * tuple.First.Definition[0].Length * tuple.Second)
            .Sum();
        var maxAvailableSpace = regionRequest.Width * regionRequest.Height;
        return maxRequiredSpace <= maxAvailableSpace;
    }

    internal class Shape(List<string> definition)
    {
        public List<string> Definition { get; } = definition;

        public long RequiredSpace { get; } = definition
            .SelectMany(line => line.ToCharArray())
            .Select(tile => ('#' == tile) ? 1 : 0)
            .Sum();
    }

    internal class RegionRequest(long width, long height, List<long> shapeCounts)
    {
        public long Width { get; } = width;
        public long Height { get; } = height;
        public List<long> ShapeCounts { get; } = shapeCounts;
    }
}