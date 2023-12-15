using System.Text;

namespace AdventOfCode._14;

public class ReflectorDish
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllTextAsync("./14/input.txt");

        var lookup = new HashSet<string>();
        var history = new List<string>();

        var cycles = 0;
        
        while (!lookup.Contains(input))
        {
            lookup.Add(input);
            history.Add(input);
            
            input = Cycle(input);
            
            cycles++;
        }

        var start = history.IndexOf(input);
        var mod = cycles - start;

        var state = history[start + (1_000_000_000 - start) % mod];
        var load = CalculateLoad(state.Split("\n")); 

        Console.WriteLine(load);
    }

    private static int CalculateLoad(string[] lines) =>
        Enumerable.Range(0, lines.Length)
            .Aggregate(0, (result, index) => result + lines[index].Count(x => x == 'O') * (lines.Length - index));

    private static string Cycle(string input) =>
        string.Join("\n", Enumerable.Range(1, 4)
            .Aggregate(input.Split("\n"), (x, i) => TiltRight(Rotate90Clockwise(x))));

    private static string[] Rotate90Clockwise(string[] input)
    {
        var rows = new string[input[0].Length];
        
        for (int i = 0; i < input[0].Length; i++)
        {
            var builder = new StringBuilder(input.Length);
            
            for (int j = 0; j < input.Length; j++)
                builder.Append(input[input.Length - 1 - j][i]);

            rows[i] = builder.ToString();
        }

        return rows;
    }

    private static string[] TiltRight(string[] input) => input
        .Select(line => string.Join("#", line
            .Split("#")
            .Select(Order)))
        .ToArray();

    private static string Order(string input)
    {
        var tmp = input.ToArray();
        Array.Sort(tmp);
        return new string(tmp);
    }
}