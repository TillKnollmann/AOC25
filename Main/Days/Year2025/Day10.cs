using LpSolveDotNet;

namespace AdventOfCode.Days.Year2025;

internal class Day10 : DayOfYear2025<Day10, List<Day10.Problem>>
{
    public override int Number => 10;

    protected override List<Problem> ParseInput()
    {
        return Input.Split(Environment.NewLine)
            .Select(line =>
            {
                var lineParts = line.Split(' ');
                var targetState = lineParts[0]
                    .Replace("[", "")
                    .Replace("]", "")
                    .ToCharArray()
                    .ToList();
                var joltages = lineParts[^1]
                    .Split(",")
                    .Select(number => int.Parse(number.Replace("{", "").Replace("}", "")))
                    .ToList();
                var buttons = lineParts
                    .Skip(1)
                    .SkipLast(1)
                    .Select((buttonString, index) =>
                    {
                        var number = index;
                        var targets = buttonString.Replace("(", "").Replace(")", "")
                            .Split(",")
                            .Select(int.Parse)
                            .ToList();
                        return new Button(number, targets);
                    })
                    .ToList();
                return new Problem(buttons, targetState, joltages);
            })
            .ToList();
    }

    public override string FirstPart()
    {
        var problems = ParsedInput.Value;
        return problems.Select(GetFewestNumberOfPresses)
            .Sum()
            .ToString();
    }
    
    public override string SecondPart()
    {
        var problems = ParsedInput.Value;
        return problems.Select(GetFewestNumberOfPressesViaIlp)
            .Sum()
            .ToString();
    }

    private static int GetFewestNumberOfPresses(Problem problem)
    {
        var powersets = GetPowersets(problem.Buttons)
            .Select(enumerable => enumerable.ToList())
            .OrderBy(powerset => powerset.Count())
            .ToList();

        foreach (var powerset in  powersets)
        {
            var initialState = GetInitialState(powerset, problem.TargetState);
            if (initialState.All(position => position.Equals('.')))
                return powerset.Count;
        }

        return -1;
    }

    /// min sum_(i=0)^(n) x_i
    /// s.t. x_i >= 0
    /// forall c_j
    /// x_i * y_ij = c_j
    /// y_ij = 1 iff button i toggles counter j 
    private static int GetFewestNumberOfPressesViaIlp(Problem problem)
    {
        var numberCounters = problem.Joltages.Count;
        var numberButtons = problem.Buttons.Count;
        
        LpSolve.Init();
        double ignored = 0;
        using var ilp = LpSolve.make_lp(numberCounters, numberButtons);
        
        ilp.set_minim();
        var objective = new double[numberButtons + 1];
        Array.Fill(objective, 1);
        objective[0] = ignored;
        ilp.set_obj_fn(objective);
        
        ilp.set_add_rowmode(true);
        for (var j = 0; j < numberCounters; j++)
        {
            var constraint = new double[numberButtons + 1];
            constraint[0] = ignored;
            for (var i = 0; i < problem.Buttons.Count; i++)
            {
                constraint[i + 1] = problem.Buttons[i].Targets.Contains(j) ? 1 : 0;
            }

            ilp.add_constraint(constraint, lpsolve_constr_types.EQ, problem.Joltages[j]);
        }
        ilp.set_add_rowmode(false); 
        
        // set integer
        for (var column = 1; column <= numberButtons; column++)
        {
            ilp.set_int(column, true);
        }
        ilp.set_verbose(lpsolve_verbosity.IMPORTANT);
        lpsolve_return solution = ilp.solve();
        if (solution == lpsolve_return.OPTIMAL)
        {
            var variables = new double[numberButtons];
            ilp.get_variables(variables);
            return variables.Select(variable => (int) variable).Sum();
        }

        throw new InvalidOperationException("No optimal solution was found");
    }
    
    private static List<char> GetInitialState(List<Button> buttons, List<char> targetState)
    {
        var state = new List<char>(targetState);
        foreach (var target in buttons.SelectMany(button => button.Targets))
        {
            state[target] = state[target] switch
            {
                '.' => '#',
                '#' => '.',
                _ => state[target]
            };
        }
        return state;
    }

    private static IEnumerable<IEnumerable<Button>> GetPowersets(List<Button> buttons)
    {
        return Enumerable.Range(0, 1 << buttons.Count)
            .Select(mask =>
                Enumerable.Range(0, buttons.Count)
                    .Where(i => (mask & (1 << i)) != 0)
                    .Select(i => buttons[i])
            );
    }

    internal class Button(int number, List<int> targets)
    {
        public int Number { get; } = number;
        public List<int> Targets { get; } = targets;

    }

    internal class Problem(List<Button> buttons, List<char> targetState, List<int> joltages)
    {
        public List<Button> Buttons { get; } = buttons;
        public List<char> TargetState { get; } = targetState;
        public List<int> Joltages { get; } = joltages;
    }
}