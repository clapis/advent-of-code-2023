namespace AdventOfCode._07;

public class CamelCards
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./07/input.txt");

        var result = input
            .Select(x => x.Split(" "))
            .Select(x => new Hand(EncodeCardsValue(x[0]), int.Parse(x[1])))
            .OrderBy(hand => hand.Value)
            .Select((hand, i) => hand.Bid * (i + 1))
            .Sum();

        Console.WriteLine(result);
    }

    // Encode cards value into a sortable string
    //           type   sequence 
    // AAA23 => 31100  1212120001
    private static string EncodeCardsValue(string cards)
        => EncodeCardsJType(cards) + EncodeCardsSequence(cards);

    // AAAAA => 50000
    // KKKK5 => 41000
    // QQQJJ => 32000
    // JJJ32 => 31100
    // TT998 => 22100
    // 88765 => 21110
    // 76543 => 11111
    private static string EncodeCardsType(string cards)
        => string.Join("", cards.ToLookup(x => x)
                .Select(x => x.Count())
                .OrderDescending())
            .PadRight(5, '0');
    
    // JJJJJ => 5
    // JJJJ* => 5
    // JJJ** => 2: 5, 11: 41
    // JJ*** => 21: 41, 111: 311
    // J**** => 4: 5, 31: 41, 22: 32, 211: 311, 1111: 2111
    private static string EncodeCardsJType(string cards)
        => (int.Parse(EncodeCardsType(cards.Replace("J", "")))
            + cards.Count(c => c == 'J') * 10000).ToString();
    
    private const string Cards = "J23456789TQKA";

    // 234TA => 0001020812
    // 22KKK => 0000111111
    private static string EncodeCardsSequence(string cards)
        => string.Join("", cards.Select(c => Cards.IndexOf(c).ToString("D2")));

    record Hand(string Value, int Bid);
}