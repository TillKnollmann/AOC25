namespace AdventOfCode.Days.Pickers;

internal class PickLatestOnlyWithSamples : IDayPicker
{
    public IEnumerable<IDay> PickDays(IReadOnlyList<IDay> days)
    {
        if (days.Where(d => !string.IsNullOrEmpty(d.Input))
                .OrderByDescending(d => d.Year)
                .ThenByDescending(d => d.Number)
                .FirstOrDefault() is { } latest)
        {
            foreach (var picked in latest.Samples().Append(latest))
            {
                yield return picked;
            }
        }
    }
}