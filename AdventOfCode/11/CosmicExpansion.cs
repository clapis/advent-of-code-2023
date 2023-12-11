namespace AdventOfCode._11;

public class CosmicExpansion
{
    const long RipsMultiplier = 1000000;

    public static async Task MainAsync()
    {
        var universe = await File.ReadAllLinesAsync("./11/input.txt");

        var xrips = FindXRips(universe).ToArray();
        var yrips = FindExpansionsY(universe).ToArray();

        var galaxies = FindGalaxies(universe).ToArray();
        
        var result = galaxies
            .SelectMany(a => galaxies
                .Select(b => (a,b)))
            .Select(pair =>
            {
                var (a, b) = pair;

                var rangex = new Range(Math.Min(a.X, b.X), Math.Max(a.X, b.X));
                var rangey = new Range(Math.Min(a.Y, b.Y), Math.Max(a.Y, b.Y));

                var ripsx = xrips.Count(x => x > rangex.Start && x < rangex.End);
                var ripsy = yrips.Count(y => y > rangey.Start && y < rangey.End);

                var dx = rangex.End - rangex.Start + ripsx * (RipsMultiplier - 1);
                var dy = rangey.End - rangey.Start + ripsy * (RipsMultiplier - 1);

                return dx + dy;
            })
            .Sum() / 2;

        Console.WriteLine(result);
    }

    private static IEnumerable<Coordinates> FindGalaxies(string[] universe)
    {
        for (var i = 0; i < universe.Length; i++)
        for (var j = 0; j < universe[i].Length; j++)
            if (universe[i][j] == '#')
                yield return new Coordinates(i, j);
    }
    
    private static IEnumerable<int> FindXRips(string[] universe)
    {
        for (var i = 0; i < universe.Length; i++)
            if (universe[i].All(x => x == '.')) yield return i;
    }
    
    private static IEnumerable<int> FindExpansionsY(string[] universe)
    {
        for (var i = 0; i < universe[0].Length; i++)
            if (universe.All(x => x[i] == '.')) yield return i;
    }
    
    private readonly record struct Coordinates(int X, int Y);
    private readonly record struct Range(int Start, int End);
}