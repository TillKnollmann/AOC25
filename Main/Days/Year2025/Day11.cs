using System.Xml;

namespace AdventOfCode.Days.Year2025;

internal class Day11 : DayOfYear2025<Day11, Day11.Graph>
{
    public override int Number => 11;

    protected override Graph ParseInput()
    {
        var graph = new Graph();
        Input.Split(Environment.NewLine)
            .ToList()
            .ForEach(line =>
            {
                var parts = line.Split(":");
                var start = parts[0].Trim();
                var targets = parts[1]
                    .Trim()
                    .Split(" ")
                    .Select(target => target.Trim())
                    .ToList();
                targets.ForEach(target => graph.AddEdge(start, target));
            });
        return graph;
    }

    public override string FirstPart()
    {
        // Strategy: Using DFS from starting node, create a topological sorting of all reachable nodes.
        // Calculate number of paths using dynamic programming based on topological sorting.
        List<string> relevantNodes =
        [
            "you",
            "out"
        ];
        var graph = ParsedInput.Value;
        if (relevantNodes.Any(node => !graph.ContainsNode(node))) return "None";
        var topologicalSorting = GetTopologicalSorting(graph, "you");
        return GetNumberOfPaths(graph, "you", "out", topologicalSorting).ToString();
    }

    public override string SecondPart()
    {
        // Strategy: Using DFS from starting node, create a topological sorting of all reachable nodes.
        // Using the topological sorting, calculate number of paths between all relevant nodes.
        // The result is the multiplication of the path counts.
        List<string> relevantNodes =
        [
            "svr",
            "dac",
            "fft",
            "out"
        ];
        var graph = ParsedInput.Value;
        if (relevantNodes.Any(node => !graph.ContainsNode(node))) return "None";
        var topologicalSorting = GetTopologicalSorting(graph, "svr");
        var orderedRelevantNodes = topologicalSorting.Where(node => relevantNodes.Contains(node)).ToList();
        var numberOfPathsThroughRelevantNodes = orderedRelevantNodes.Take(orderedRelevantNodes.Count - 1)
            .Zip(orderedRelevantNodes.Skip(1))
            .Select((tuple, _) => GetNumberOfPaths(graph, tuple.First, tuple.Second, topologicalSorting))
            .Aggregate(1L, (acc, val) => acc * val);

        return numberOfPathsThroughRelevantNodes.ToString();
    }

    private static long GetNumberOfPaths(Graph graph, string start, string target, List<string> topologicalSorting)
    {
        var nodesToNumberOfPaths = new Dictionary<string, long> { { start, 1 } };
        foreach (var node in topologicalSorting.Where(node => !nodesToNumberOfPaths.ContainsKey(node)))
        {
            nodesToNumberOfPaths.Add(node, graph.GetPredecessorNodes(node)
                .Select(predecessor => nodesToNumberOfPaths.GetValueOrDefault(predecessor, 0))
                .Sum());
            if (node == target)
                break;
        }

        return nodesToNumberOfPaths.GetValueOrDefault(target, 0);
    }

    private static List<string> GetTopologicalSorting(Graph graph, string startNode)
    {
        var topologicalSorting = new List<string>();
        DfsRecursive(graph, startNode, new HashSet<string>(), finishedNode => topologicalSorting.Add(finishedNode));
        topologicalSorting.Reverse();
        return topologicalSorting;
    }

    private static void DfsRecursive(Graph graph, string node, HashSet<string> visited, Action<string> onNodeFinished)
    {
        if (!visited.Add(node)) return;

        foreach (var neighbor in graph.GetAdjacentNodes(node))
        {
            DfsRecursive(graph, neighbor, visited, onNodeFinished);
        }

        onNodeFinished.Invoke(node);
    }

    internal class Graph
    {
        private readonly Dictionary<string, List<string>> _adjacencyList = new();
        private readonly Dictionary<string, List<string>> _predecessorList = new();

        public List<string> GetPredecessorNodes(string node)
        {
            return _predecessorList.GetValueOrDefault(node, []);
        }

        public List<string> GetAdjacentNodes(string node)
        {
            return _adjacencyList.GetValueOrDefault(node, []);
        }

        public bool ContainsNode(string node)
        {
            return _adjacencyList.ContainsKey(node) || _predecessorList.ContainsKey(node);
        }

        public void AddEdge(string start, string target)
        {
            AddToAdjacencyList(start, target);
            AddToPredecessorList(target, start);
        }

        private void AddToAdjacencyList(string start, string target)
        {
            if (_adjacencyList.TryGetValue(start, out var value))
            {
                value.Add(target);
            }
            else
            {
                _adjacencyList.Add(start, [target]);
            }
        }

        private void AddToPredecessorList(string node, string predecessor)
        {
            if (_predecessorList.TryGetValue(node, out var value))
            {
                value.Add(predecessor);
            }
            else
            {
                _predecessorList.Add(node, [predecessor]);
            }
        }
    }
}