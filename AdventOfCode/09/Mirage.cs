namespace AdventOfCode._09;

public class Mirage
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./09/input.txt");

        var result = input
            .Select(line => line.Split(" ").Select(int.Parse).ToArray())
            .Select(CalculateProjection)
            .Sum();

        Console.WriteLine(result);
    }

    private static int CalculateProjection(int[] input)
    {
        if (input.All(x => x == 0))
            return 0;

        return input[0] - CalculateProjection(Reduce(input));
    }

    private static int[] Reduce(int[] input)
    {
        var result = new int[input.Length - 1];

        for (int i = 1; i < input.Length; i++)
            result[i - 1] = input[i] - input[i - 1];

        return result;
    }
}