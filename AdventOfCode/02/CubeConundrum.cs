namespace AdventOfCode._02;

public class CubeConundrum
{
    private static readonly StringSplitOptions StringSplitDefaultOption =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    public static async Task MainAsync()
    {
        var lines = await File.ReadAllLinesAsync("./02/input.txt");

        var total = lines
            .Select(game => game
                .Split(":", StringSplitDefaultOption)[1]
                .Split(";", StringSplitDefaultOption)
                .SelectMany(draw => draw.Split(",", StringSplitDefaultOption))
                .Aggregate((0, 0, 0), (result, cubes) => (
                    Math.Max(result.Item1, GetColorValueOrDefault(cubes, "red")),
                    Math.Max(result.Item2, GetColorValueOrDefault(cubes, "green")),
                    Math.Max(result.Item3, GetColorValueOrDefault(cubes, "blue"))
                )))
            .Aggregate(0, (result, entry) => result + entry.Item1 * entry.Item2 * entry.Item3);

        Console.WriteLine(total);
    }

    private static int GetColorValueOrDefault(string input, string color)
        => input.EndsWith(color) ? int.Parse(input[..^color.Length]) : 0;
    
}