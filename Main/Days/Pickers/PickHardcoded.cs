namespace AdventOfCode.Days.Pickers;

internal class PickHardcoded : IDayPicker
{
    public IEnumerable<IDay> PickDays(IReadOnlyList<IDay> days) => days
        .Where(d => !string.IsNullOrEmpty(d.Input))
        .Where(d => d.Year == 2024)
        .Where(d => d.Number == 24)
        .SelectMany(d => d.Samples().Append(d))
        .Where(d => d.SampleNumber == 0);
}