namespace AdventOfCode;

internal enum Stage
{
    Parsing,
    FirstPart,
    SecondPart
}

internal static class StageExtensions
{
    internal static string ToShortLabel(this Stage stage) => stage switch
    {
        Stage.Parsing => "P",
        Stage.FirstPart => "I",
        Stage.SecondPart => "II",
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
    };
    
    internal static string ToLongLabel(this Stage stage) => stage switch
    {
        Stage.Parsing => "Parsing",
        Stage.FirstPart => "First Part",
        Stage.SecondPart => "Second Part",
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
    };
    
    internal static ConsoleColor ToColor(this Stage stage) => stage switch
    {
        Stage.Parsing => ConsoleColor.DarkBlue,
        Stage.FirstPart => ConsoleColor.DarkMagenta,
        Stage.SecondPart => ConsoleColor.DarkCyan,
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
    };
}