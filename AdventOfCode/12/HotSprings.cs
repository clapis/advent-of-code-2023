namespace AdventOfCode._12;

public class HotSprings
{
    private static readonly Dictionary<string, long> Cache = new();

    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./12/input.txt");

        var result = input
            .Select(CountValidArrangements)
            .Sum();

        Console.WriteLine(result);
    }

    private static long CountValidArrangements(string input)
    {
        var parts = input.Split();

        var conditions = Unfold(parts[0], '?');
        var blocks = Unfold(parts[1], ',')
            .Split(",")
            .Select(int.Parse)
            .ToArray();

        return CountValidArrangements(conditions, blocks);
    }

    private static long CountValidArrangements(string input, int[] blocks)
    {
        if (input is "")
            return blocks.Length == 0 ? 1 : 0;

        if (blocks.Length == 0)
            return input.Contains("#") ? 0 : 1;

        var key = $"{input} {string.Join(",", blocks)}";

        if (Cache.TryGetValue(key, out var count))
            return count;

        var result = 0L;
        
        if (input[0] is '.' or '?')
            result += CountValidArrangements(input[1..], blocks);

        if (input[0] is '#' or '?')
        {
            if (input.Length >= blocks[0] && 
                input[..blocks[0]].All(c => c is '#' or '?') && 
                (input.Length == blocks[0] || input[blocks[0]] is not '#'))
            {
                var next = input.Length > blocks[0] ? input[(blocks[0] + 1)..] : string.Empty;
                result += CountValidArrangements(next, blocks[1..]);
            }
        }

        Cache[key] = result;

        return result;
    }
    
    private static string Unfold(string input,char joint)
        => string.Join(joint, Enumerable.Repeat(input, 5));
}