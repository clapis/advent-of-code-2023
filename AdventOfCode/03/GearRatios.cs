namespace AdventOfCode._03;

public class GearRatios
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllTextAsync("./03/input.txt");
        
        var grid = input.Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToArray();

        Dictionary<Gear, HashSet<Part>> map = new();

        for (int row = 0; row < grid.Length; row++)
        {
            for (int column = 0; column < grid[row].Length; column++)
            {
                // Skip until first digit is found
                if (!char.IsDigit(grid[row][column]))
                    continue;

                // Find number's range 
                var range = FindNumberRange(grid[row], column);
                
                // Parse number
                var number = int.Parse(grid[row][range]);
                
                var part = new Part(number, row, column);
                
                // Given the number's range, search for adjacent gears
                var gears = FindAdjacentGears(grid, row, range);
                
                // Store gears and adjacent parts
                foreach (var gear in gears)
                {
                    if (!map.ContainsKey(gear))
                        map[gear] = new HashSet<Part>();

                    map[gear].Add(part);
                }

                // Move column index forward to the end of the number
                column = range.End.Value - 1;
            }
        }

        var total = map
            .Select(x => x.Value)
            .Where(x => x.Count == 2)
            .Aggregate(0, (result, parts) => result + parts.Aggregate(1, (ratio, part) => ratio * part.Number));

        Console.WriteLine(total);
    }

    private static Range FindNumberRange(char[] line, int start)
    {
        var end = start + 1;

        while (end < line.Length && char.IsDigit(line[end])) end++;

        return start..end;
    }

    private static HashSet<Gear> FindAdjacentGears(char[][] grid, int row, Range range) => Enumerable
        .Range(range.Start.Value, range.End.Value - range.Start.Value)
        .SelectMany(column => FindAdjacentGears(grid, row, column))
        .ToHashSet();

    private static IEnumerable<Gear> FindAdjacentGears(char[][] grid, int row, int column)
    {
        for (int a = row - 1; a <= row + 1; a++)
        {
            if (a < 0 || a >= grid.Length)
                continue;
            
            for (int b = column - 1; b <= column + 1 ; b++)
            {
                if (b < 0 || b >= grid[0].Length)
                    continue;

                if (a == row && b == column)
                    continue;

                var symbol = grid[a][b];
                if (symbol == '*')
                    yield return new Gear(a, b);
            }
        }
    }

    private record Gear(int Row, int Column);
    private record Part(int Number, int Row, int Column);
}