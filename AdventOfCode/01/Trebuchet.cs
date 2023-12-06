using System.Text.RegularExpressions;

namespace AdventOfCode._01;

public class Trebuchet
{
    const string Pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

    public static async Task MainAsync()
    {
        var lines = await File.ReadAllLinesAsync("./01/input.txt");

        var calibrations = lines.Select(line =>
        {
            var first = Regex.Match(line, Pattern);
            var last = Regex.Match(line, Pattern, RegexOptions.RightToLeft);

            return Parse(first.Value) * 10 + Parse(last.Value);
        });
        
        Console.WriteLine(calibrations.Sum());
    }

    private static int Parse(string input) => input switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => int.Parse(input)
    };
}