namespace AdventOfCode.Days.Year2025;

internal class Day08 : DayOfYear2025<Day08, List<Day08.Point>>
{
    public override int Number => 8;

    protected override List<Point> ParseInput()
    {
        return Input.Split(Environment.NewLine)
            .Select(line => line.Split(","))
            .Select(line => new Point(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2])))
            .ToList();
    }

    public override string FirstPart()
    {
        var nodes = ParsedInput.Value;
        var limit = nodes.Count == 20 ? 10 : 1000;
        var (unionFind, _) = RunKruskal(nodes, tuple => tuple.Item1 == limit); // limit reached
        return unionFind.Unions.OrderByDescending(pair => pair.Value.Count).Take((3)) // take three largest
            .Aggregate(1L, (acc, val) => acc * val.Value.Count) // multiply sizes
            .ToString();
    }

    public override string SecondPart()
    {
        var nodes = ParsedInput.Value;
        var (_, nullableNextEdge) = RunKruskal(nodes, tuple =>
            tuple.Item2.Unions.Count == 2 // only two circuits remaining
            && tuple.Item3 != null // there is a next edge
            && !tuple.Item2.Find(tuple.Item3.Value.Item1)
                .Equals(tuple.Item2.Find(tuple.Item3.Value.Item2))); // next edge combines two circuits
        return (nullableNextEdge?.Item1.X * nullableNextEdge?.Item2.X)?.ToString() ?? "";
    }

    static (UnionFind<Point>, (Point, Point, double)?) RunKruskal(List<Point> nodes,
        Predicate<(int, UnionFind<Point>, (Point, Point, double)?)> stopCriterion)
    {
        var edges = GetSortedWeightedEdges(nodes);
        var unionFind = new UnionFind<Point>();
        nodes.ForEach(node => unionFind.MakeSet(node));
        var stepCount = 0;
        (Point, Point, double)? edgeAfterStop = null;
        for (var index = 0; index < edges.Count; index++)
        {
            var edge = edges[index];
            edgeAfterStop = index + 1 < edges.Count ? edges[index + 1] : null;
            unionFind.Union(edge.Item1, edge.Item2);
            stepCount++;
            if (!stopCriterion.Invoke((stepCount, unionFind, edgeAfterStop))) continue;
            break;
        }

        return (unionFind, edgeAfterStop);
    }

    static List<(Point, Point, double)> GetSortedWeightedEdges(List<Point> nodes)
    {
        List<(Point, Point, double)> edges = [];
        for (var indexA = 0; indexA < nodes.Count; indexA++)
        {
            var firstNode = nodes[indexA];
            for (var indexB = indexA + 1; indexB < nodes.Count; indexB++)
            {
                var secondNode = nodes[indexB];
                edges.Add((firstNode, secondNode, GetDistance(firstNode, secondNode)));
            }
        }

        return edges.OrderBy(tuple => tuple.Item3).ToList();
    }

    static double GetDistance(Point pointA, Point pointB)
    {
        return GetEuclideanDistance(pointA, pointB);
    }

    static double GetEuclideanDistance(Point pointA, Point pointB)
    {
        return Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2) +
                         Math.Pow(pointA.Z - pointB.Z, 2));
    }

    internal class Point(int x, int y, int z) : IEquatable<Point>
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;

        public bool Equals(Point? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    private class UnionFind<T> where T : IEquatable<T>
    {
        public Dictionary<T, List<UnionFindNode<T>>> Unions { get; } = new();
        private readonly Dictionary<T, UnionFindNode<T>> _nodeToInternalNode = new();

        public void MakeSet(T element)
        {
            UnionFindNode<T> unionFindNode = new UnionFindNode<T>(element);
            Unions.Add(element, [unionFindNode]);
            _nodeToInternalNode.Add(element, unionFindNode);
        }

        public void Union(T element1, T element2)
        {
            var internalElement1 = _nodeToInternalNode[element1];
            var internalElement2 = _nodeToInternalNode[element2];
            var element1Representation = internalElement1.Representation;
            var element2Representation = internalElement2.Representation;
            if (element1Representation.Equals(element2Representation))
                return;
            var element1Set = Unions[element1Representation];
            var element2Set = Unions[element2Representation];
            List<UnionFindNode<T>> combinedList = [];
            if (element1Set.Count < element2Set.Count)
            {
                foreach (var unionFindNode in element1Set)
                {
                    unionFindNode.Representation = element2Representation;
                }

                combinedList.AddRange(element2Set.Union(element1Set));
            }
            else
            {
                foreach (var unionFindNode in element2Set)
                {
                    unionFindNode.Representation = element1Representation;
                }

                combinedList.AddRange(element1Set.Union(element2Set));
            }

            Unions.Remove(element1Representation);
            Unions.Remove(element2Representation);
            Unions.Add(combinedList.First().Representation, combinedList);
        }

        public T Find(T element)
        {
            return _nodeToInternalNode[element].Representation;
        }
    }

    private class UnionFindNode<T>(T representation)
    {
        public T Representation { set; get; } = representation;
    }
}