namespace AdventOfCode.Days.Year2025;

internal class Day03 : DayOfYear2025<Day03, List<Day03.Bank>>
{
    public override int Number => 3;

    protected override List<Bank> ParseInput()
    {
        return Input.Split("\n")
            .Select(Bank.Parse)
            .ToList();
    }

    public override string FirstPart()
    {
        return ParsedInput.Value
            .Select(bank => bank.GetHighestJolt(2))
            .Sum()
            .ToString();
    }

    public override string SecondPart()
    {
        return ParsedInput.Value
            .Select(bank => bank.GetHighestJolt(12))
            .Sum()
            .ToString();
    }

    internal class Bank
    {
        private Bank(int[] batteries)
        {
            Batteries = batteries;
        }

        private int[] Batteries { get; }

        public static Bank Parse(string input)
        {
            return new Bank(input.Trim().ToCharArray()
                .Select(character => int.Parse(character.ToString()))
                .ToArray());
        }

        public long GetHighestJolt(int digits)
        {
            int[] selectedDigits = [];
            var lastSelectedDigitIndex = -1;
            for (var i = 1; i <= digits; i++)
            {
                (lastSelectedDigitIndex, var lastSelectedDigit) = Batteries
                    .Index()
                    .Skip(lastSelectedDigitIndex + 1)
                    .Take(Batteries.Length - (lastSelectedDigitIndex + 1) - (digits - i))
                    .MaxBy(tuple => tuple.Item);
                selectedDigits = selectedDigits.Append(lastSelectedDigit).ToArray();
            }

            return long.Parse(string.Join("", selectedDigits));
        }
    }
}