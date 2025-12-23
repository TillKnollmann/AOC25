using AdventOfCode.Days;
using AdventOfCode.Days.Pickers;
using MrMeeseeks.DIE.Configuration.Attributes;

namespace AdventOfCode;

[ImplementationChoice(typeof(IDayPicker), typeof(PickHardcodedWithSamples))]
[DecoratorSequenceChoice(typeof(IDay), typeof(IDay), typeof(DayDecoratorTrackTime), typeof(DayDecoratorPrintSolution))]
[CreateFunction(typeof(IProgram), "Create")]
[CreateFunction(typeof(Func<int, long, long?, PickSpecific>), "CreatePickSpecific")]
[CreateFunction(typeof(IReadOnlyList<IDay>), "CreateDays")]
internal sealed partial class Container;