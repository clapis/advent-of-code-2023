namespace AdventOfCode._07;

public class CamelCards
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./07/input.txt");

        var result = input
            .Select(x => x.Split(" "))
            .Select(x => new Hand(Cards: ParseCards(x[0]), Bid: int.Parse(x[1])))
            .OrderDescending(new HandComparer())
            .Select((hand, i) => hand.Bid * (i + 1))
            .Sum();

        Console.WriteLine(result);
    }

    class HandComparer : Comparer<Hand>
    {
        public override int Compare(Hand? x, Hand? y)
        {
            // First rule: compare type
            var typeComparison = x.Type.CompareTo(y.Type);
            
            if (typeComparison != 0) return typeComparison;
            
            // Second rule: find first strongest card
            var (cx, cy) = x.Cards.Zip(y.Cards)
                .SkipWhile(e => e.First == e.Second)
                .First();

            return cx.CompareTo(cy);
        }
    }
    
    private static HandType GetHandType(Card[] cards)
    {
        var jcount = cards.Count(x => x == Card.CJ);
        
        var lookup = cards
            .Where(x => x != Card.CJ)
            .ToLookup(x => x);

        if (jcount == 5 ||
            lookup.Any(x => x.Count() + jcount == 5))
            return HandType.FiveOfAKind;

        if (lookup.Any(x => x.Count() + jcount == 4))
            return HandType.FourOfAKind;
        
        if (lookup.Any(x => x.Count() == 3) && 
            lookup.Any(x => x.Count() == 2))
            return HandType.FullHouse;
            
        if (lookup.Count(x => x.Count() == 2) == 2)
            return jcount > 0 ? HandType.FullHouse : HandType.TwoPairs;

        if (lookup.Any(x => x.Count() + jcount == 3))
            return HandType.ThreeOfAKind;
        
        if (lookup.Any(x => x.Count() + jcount == 2))
            return HandType.OnePair;

        return HandType.HighCard;
    }

    private static Card[] ParseCards(string cards) => cards.ToCharArray()
            .Select(x => Enum.Parse<Card>($"C{x}"))
            .ToArray();
    
    record Hand(Card[] Cards, int Bid)
    {
        public HandType Type { get; } = GetHandType(Cards);
    };
    
    enum HandType { FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPairs, OnePair, HighCard };
    enum Card { CA, CK, CQ, CT, C9, C8, C7, C6, C5, C4, C3, C2, CJ };
}