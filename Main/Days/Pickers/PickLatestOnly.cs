namespace AdventOfCode.Days.Pickers;

internal class PickLatestOnly : IDayPicker
{
    public IEnumerable<IDay> PickDays(IReadOnlyList<IDay> days)
    {
        if (days.Where(d => !string.IsNullOrEmpty(d.Input))
                .OrderByDescending(d => d.Year)
                .ThenByDescending(d => d.Number)
                .FirstOrDefault() is { } latest)
        {
            yield return latest;
        }
    }
}