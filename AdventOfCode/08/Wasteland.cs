using System.Text.RegularExpressions;

namespace AdventOfCode._08;

public class Wasteland
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./08/input.txt");

        var directions = input[0]
            .Select(c => c == 'R' ? 1 : 0)
            .ToArray();

        var map = input[2..]
            .Select(x => Regex
                .Matches(x, @"\w{3}")
                .Select(x => x.Value)
                .ToArray())
            .ToDictionary(x => x[0], x => x[1..]);

        var result = map.Keys.Where(x => x.EndsWith("A"))
            .Select(start =>
            {
                var steps = 0;
                var current = start;

                while (!current.EndsWith("Z"))
                {
                    var next = directions[steps % directions.Length];

                    current = map[current][next];

                    steps++;
                }

                return (long)steps;
            })
            .Aggregate(1L, LeastCommonMultiple);

        Console.WriteLine(result);
    }

    static long LeastCommonMultiple (long a, long b) 
        => a * b / GreatestCommonFactor(a, b);

    static long GreatestCommonFactor(long a, long b)
        => b == 0 ? a : GreatestCommonFactor(b, a % b);
}