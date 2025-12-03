namespace AdventOfCode;

internal enum FourDirections { North, East, South, West }

internal static class FourDirectionsUtils
{
    internal static IEnumerable<(int X, int Y)> GetNeighbors((int X, int Y) currentPosition)
    {
        yield return (currentPosition.X - 1, currentPosition.Y);
        yield return (currentPosition.X + 1, currentPosition.Y);
        yield return (currentPosition.X, currentPosition.Y - 1);
        yield return (currentPosition.X, currentPosition.Y + 1);
    }
    
    internal static IEnumerable<(long X, long Y)> GetNeighbors((long X, long Y) currentPosition)
    {
        yield return (currentPosition.X - 1, currentPosition.Y);
        yield return (currentPosition.X + 1, currentPosition.Y);
        yield return (currentPosition.X, currentPosition.Y - 1);
        yield return (currentPosition.X, currentPosition.Y + 1);
    }
    
    internal static (long X, long Y) MakeAStep((long X, long Y) currentPosition, FourDirections currentDirection) =>
        currentDirection switch
        {
            FourDirections.North => (currentPosition.X, currentPosition.Y - 1),
            FourDirections.East => (currentPosition.X + 1, currentPosition.Y),
            FourDirections.South => (currentPosition.X, currentPosition.Y + 1),
            FourDirections.West => (currentPosition.X - 1, currentPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
    
    internal static (long X, long Y) MakeSteps((long X, long Y) currentPosition, FourDirections currentDirection, long stepCount) =>
        currentDirection switch
        {
            FourDirections.North => (currentPosition.X, currentPosition.Y - stepCount),
            FourDirections.East => (currentPosition.X + stepCount, currentPosition.Y),
            FourDirections.South => (currentPosition.X, currentPosition.Y + stepCount),
            FourDirections.West => (currentPosition.X - stepCount, currentPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
    
    internal static (int X, int Y) MakeAStep((int X, int Y) currentPosition, FourDirections currentDirection) =>
        currentDirection switch
        {
            FourDirections.North => (currentPosition.X, currentPosition.Y - 1),
            FourDirections.East => (currentPosition.X + 1, currentPosition.Y),
            FourDirections.South => (currentPosition.X, currentPosition.Y + 1),
            FourDirections.West => (currentPosition.X - 1, currentPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
    
    internal static (int X, int Y) MakeSteps((int X, int Y) currentPosition, FourDirections currentDirection, int stepCount) =>
        currentDirection switch
        {
            FourDirections.North => (currentPosition.X, currentPosition.Y - stepCount),
            FourDirections.East => (currentPosition.X + stepCount, currentPosition.Y),
            FourDirections.South => (currentPosition.X, currentPosition.Y + stepCount),
            FourDirections.West => (currentPosition.X - stepCount, currentPosition.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
    
    internal static FourDirections RotateCounterClockwise(this FourDirections currentDirection) =>
        currentDirection switch
        {
            FourDirections.North => FourDirections.West,
            FourDirections.West => FourDirections.South,
            FourDirections.South => FourDirections.East,
            FourDirections.East => FourDirections.North,
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
    
    internal static FourDirections RotateClockwise(this FourDirections currentDirection) =>
        currentDirection switch
        {
            FourDirections.North => FourDirections.East,
            FourDirections.East => FourDirections.South,
            FourDirections.South => FourDirections.West,
            FourDirections.West => FourDirections.North,
            _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
        };
}