namespace AdventOfCode._13;

public class Incidence
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllTextAsync("./13/input.txt");

        var result = input.Split("\n\n")
            .Select(Calculate)
            .Sum();

        Console.WriteLine(result);
    }

    private static int Calculate(string pattern)
    {
        var rows = pattern.Split("\n");

        return GetPoi(rows) * 100 + GetPoi(Invert(rows));
    }

    private static int GetPoi(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            var top = lines[..i];
            var bottom = lines[i..];

            var count = top.Reverse().Zip(bottom)
                .Sum(x => x.First.Zip(x.Second)
                    .Count(y => y.First != y.Second));

            if (count == 1)
                return i;
        }

        return 0;
    }

    private static string[] Invert(string[] input)
    {
        var result = new string[input[0].Length];
        
        for (int i = 0; i < input[0].Length; i++)
            result[i] = string.Join("", input.Select(line => line[i]));

        return result;
    }
}