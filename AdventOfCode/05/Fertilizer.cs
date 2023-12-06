namespace AdventOfCode._05;

public class Fertilizer
{
    public static async Task MainAsync()
    {
        var almanac = await Parser.ReadInputAsync("./05/input.txt");

        var result = almanac.Seeds
            .SelectMany(range => ApplyMappings(almanac.Maps, range))
            .Min(range => range.Start);

        Console.WriteLine(result);
    }
    
    private static IEnumerable<Range> ApplyMappings(List<AlmanacMap> maps, Range seed) =>
        maps.Aggregate(new List<Range> { seed }, (ranges, map) =>
        {
            var mapped = new List<Range>();
            
            var unmapped = map.Rules.Aggregate(ranges, (ranges, rule) =>
            {
                mapped.AddRange(ranges
                    .Where(r => r.Intersects(rule.Source))
                    .Select(r => r.Intersection(rule.Source))
                    .Select(r => r.Shift(rule.Destination.Start - rule.Source.Start)));

                return ranges
                    .Where(r => !r.Intersects(rule.Source))
                    .Union(ranges
                        .Where(r => r.Intersects(rule.Source))
                        .SelectMany(r => r.Except(rule.Source))
                    )
                    .ToList();
            });

            return mapped.Union(unmapped).ToList();
        });
}

