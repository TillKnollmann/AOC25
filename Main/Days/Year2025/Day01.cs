namespace AdventOfCode.Days.Year2025;

internal class Day01 : DayOfYear2025<Day01, List<Day01.Command>>
{
    public override int Number => 1;

    public override string FirstPart()
    {
        var initialPosition = 50;
        var commands = ParsedInput.Value;
        var currentPosition = initialPosition;
        var numberOfZeros = 0;
        foreach (var cmd in commands)
        {
            currentPosition = ApplyRule(currentPosition, cmd.GetCount(), cmd.IsLeft());
            if (currentPosition == 0)
                numberOfZeros++;
        }

        return numberOfZeros.ToString();
    }

    public override string SecondPart()
    {
        var initialPosition = 50;
        var commands = ParsedInput.Value;
        var currentPosition = initialPosition;
        var numberOfClicks = 0;
        foreach (var cmd in commands)
        {
            if (currentPosition == 0 && cmd.IsLeft())
                currentPosition = 100; // ensure first underflow is not count
            if (cmd.IsLeft() && cmd.GetCount() > currentPosition)
            {
                var test = cmd.GetCount();
                while (test > currentPosition)
                {
                    numberOfClicks++;
                    test -= 100;
                }
            }
            else if (!cmd.IsLeft() && cmd.GetCount() > 100 - currentPosition)
            {
                var test = cmd.GetCount();
                while (test > 100 - currentPosition)
                {
                    numberOfClicks++;
                    test -= 100;
                }
            }

            currentPosition = ApplyRule(currentPosition, cmd.GetCount(), cmd.IsLeft());
            if (currentPosition == 0)
                numberOfClicks++;
        }

        return numberOfClicks.ToString();
    }

    protected override List<Command> ParseInput()
    {
        return Input.Split('\n')
            .Select((line, index) => Command.ByString(line))
            .ToList();
    }

    private static int ApplyRule(int current, int count, bool left)
    {
        var toAdd = count % 100;
        if (left)
            toAdd *= -1;

        return (current + toAdd + 100) % 100;
    }

    internal class Command(int count, bool left)
    {
        public static Command ByString(string input)
        {
            input = input.Trim();
            var left = input[0] == 'L';
            var count = int.Parse(input.Substring(1));
            return new Command(count, left);
        }

        public int GetCount()
        {
            return count;
        }

        public bool IsLeft()
        {
            return left;
        }
    }
}