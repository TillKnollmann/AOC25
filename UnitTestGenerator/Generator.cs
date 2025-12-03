using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace AdventOfCode.UnitTestGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var labelsAndSolutions = context.AdditionalTextsProvider.Where(f => f.Path.EndsWith("Solutions.resx"))
            .SelectMany((f, _) =>
            {
                var document = XDocument.Load(f.Path);
                return document.Root.Elements("data")
                    .Select(d => (DayLabel: d.Attribute("name").Value, Solution: d.Element("value").Value));
            })
            .Collect();

        context.RegisterSourceOutput(labelsAndSolutions, (spc, s) =>
        {
            var code = new StringBuilder();
            code.AppendLine(
                """
                namespace AdventOfCode.UnitTests;

                public class Tests
                {
                """);
            foreach (var (dayLabel, solution) in s)
            {
                var dayNumber = int.Parse(dayLabel[..2]);
                var sampleNumber = dayLabel[2] == '.' ? int.Parse(dayLabel[3..5]) : 0;
                var part = dayLabel.EndsWith("II") ? "Second" : "First";
                var sampleTestName = sampleNumber > 0 ? $"_Sample{sampleNumber}" : "";
                code.AppendLine(
                    $$"""
                          [global::Xunit.FactAttribute]
                          public void Day{{dayNumber.ToString().PadLeft(2, '0')}}{{sampleTestName}}_{{part}}Part()
                          {
                              // Arrange
                              var container = global::AdventOfCode.Container.DIE_CreateContainer();
                              var dayPicker = container.CreatePickSpecific()({{dayNumber}}, {{sampleNumber}});
                              var days = container.CreateDays();
                              var day = dayPicker.PickDay(days).Single();
                              
                              const string expected = "{{solution}}";
                              
                              // Act
                              var result = day.{{part}}Part();
                              
                              // Assert
                              global::Xunit.Assert.Equal(expected, result);
                          }
                      """);
            }
            code.AppendLine("}");
            spc.AddSource("Tests.g.cs", code.ToString());
        });
    }
}