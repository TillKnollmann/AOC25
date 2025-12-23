namespace AdventOfCode.Days.Pickers;

internal class PickHardcodedWithSamples : IDayPicker
{
    public IEnumerable<IDay> PickDays(IReadOnlyList<IDay> days) => days
        .Where(d => !string.IsNullOrEmpty(d.Input))
        .Where(d => d.Year == 2025)
        .Where(d => d.Number == 10)
        .SelectMany(d => d.Samples().Append(d));
}