using System.Text.RegularExpressions;

namespace AdventOfCode._06;

public class BoatRace
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./06/input.txt");

        var races = Read(input[0]).Zip(Read(input[1]));

        var result = races
            .Aggregate(1.0, (result, race) =>
            {
                // Bhaskara
                // 7 = hold + travel-time
                // travel-time * speed > 9
                // -hold^2 + 7hold > 9
                
                var (time, distance) = race;
                
                var (first, second) = GetQuadraticSolutions(-1, time, -distance);

                var start = Math.Floor(first) + 1;
                var end = Math.Ceiling(second) - 1;
                
                return result * (end - start + 1);
            });
        
        Console.WriteLine(result);
    }

    private static (double, double) GetQuadraticSolutions(long a, long b, long c)
    {
        var delta = Math.Sqrt(Math.Pow(b, 2) - 4 * a * c);

        return ((-b + delta) / 2 * a, (-b - delta) / 2 * a);
    }

    private static long[] Read(string input)
        => Regex.Matches(input, @"\d+").Select(x => long.Parse(x.Value)).ToArray();
}