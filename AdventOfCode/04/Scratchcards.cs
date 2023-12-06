namespace AdventOfCode._04;

public class Scratchcards
{
    public static async Task MainAsync()
    {
        var lines = await File.ReadAllLinesAsync("./04/input.txt");

        var cards = lines.Select(GetWinningNumbersCountFromCard).ToArray();
        var copies = new int[cards.Length];

        var total = 0;
        
        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = i + 1; j < i + 1 + cards[i] && j < cards.Length; j++)
                copies[j] += copies[i] + 1;
            
            total += copies[i] + 1;
        }

        Console.WriteLine(total);
    }

    private static int GetWinningNumbersCountFromCard(string input)
    {
        var lists = input
            .Split(":")[1]
            .Split("|");

        var winning = ParseNumberList(lists[0]);
        var having = ParseNumberList(lists[1]);

        return winning.Intersect(having).Count();
    }
    
    private static HashSet<int> ParseNumberList(string input)
    {
        var parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        return parts.Select(int.Parse).ToHashSet();
    }
}