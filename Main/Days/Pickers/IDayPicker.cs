namespace AdventOfCode.Days.Pickers;

internal interface IDayPicker
{
    IEnumerable<IDay> PickDays(IReadOnlyList<IDay> days);
}