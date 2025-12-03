namespace AdventOfCode.Days;

internal enum ResultStatus
{
    Incorrect,
    Uncertain,
    Correct
}

internal static class ResultStatusExtensions
{
    internal static char ToChar(this ResultStatus resultStatus) => resultStatus switch
    {
        ResultStatus.Incorrect => 'X',
        ResultStatus.Uncertain => '?',
        ResultStatus.Correct => '\u2713',
        _ => '_'
    };

    internal static ConsoleColor ToConsoleColor(this ResultStatus resultStatus) => resultStatus switch
    {
        ResultStatus.Incorrect => ConsoleColor.DarkRed,
        ResultStatus.Uncertain => ConsoleColor.DarkYellow,
        ResultStatus.Correct => ConsoleColor.DarkGreen,
        _ => ConsoleColor.DarkGray,
    };
}

internal static class CharExtensions
{
    internal static ResultStatus ToResultStatus(this char character) => character switch
    {
        'X' => ResultStatus.Incorrect,
        '?' => ResultStatus.Uncertain,
        '\u2713' => ResultStatus.Correct,
        _ => (ResultStatus)99
    };
}