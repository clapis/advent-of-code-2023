
using System.Text.RegularExpressions;

namespace AdventOfCode._05;

public static class Parser
{
    public static async Task<Almanac> ReadInputAsync(string filename)
    {
        var contents = await File.ReadAllTextAsync(filename);
        
        var sections = contents.Split("\n\n");
        
        var seeds = ParseSeeds(sections[0]);
        var maps = ParseMaps(sections[1..]);

        return new Almanac(seeds, maps.ToList());
    }

    private static List<Range> ParseSeeds(string line) => Regex
        .Matches(line[6..], @"\d+")
        .Select(match => long.Parse(match.Value))
        .Chunk(2)
        .Select(range => new Range(range[0], range[0] + range[1]))
        .ToList();

    private static List<AlmanacMap> ParseMaps(string[] sections) => sections
        .Select(section => new AlmanacMap(section
            .Split("\n")[1..]
            .Select(ParseRule)
            .ToList())
        ).ToList();

    private static AlmanacMapRule ParseRule(string rule)
    {
        var parts = rule
            .Split(" ")
            .Select(long.Parse)
            .ToArray();

        var source = new Range(parts[1], parts[1] + parts[2]);
        var destination = new Range(parts[0], parts[0] + parts[2]);

        return new AlmanacMapRule(source, destination);
    }
}