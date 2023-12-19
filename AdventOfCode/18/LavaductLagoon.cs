namespace AdventOfCode._18;

public class LavaductLagoon
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./18/input.txt");
        
        var steps = ParseInput2(input);
        
        var result = GetLoopTotalArea(steps);
        
        Console.WriteLine(result);
    }

    private static long GetLoopTotalArea(IEnumerable<(int RowOffset, int ColumnOffset)> steps)
    {
        var coordinates = GetLoopCoordinates(steps).ToArray();

        // https://en.wikipedia.org/wiki/Shoelace_formula
        // Determine the area of a polygon by its vertices 
        var area = 0L;
        
        for (var i = 0; i < coordinates.Length; i++)
            area += (long) coordinates[(i + 1) % coordinates.Length].Column * coordinates[i].Row -
                    (long) coordinates[(i + 1) % coordinates.Length].Row * coordinates[i].Column;

        area = Math.Abs(area / 2);

        var lenght = GetLoopLenght(steps);

        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        // Determine number of points interior to the polygon
        return area + 1 - lenght / 2 + lenght;
    }
    
    private static int GetLoopLenght(IEnumerable<(int RowOffset, int ColumnOffset)> steps)
        => steps.Select(x =>  Math.Abs(x.RowOffset + x.ColumnOffset)).Sum();
    private static IEnumerable<(int Row, int Column)> GetLoopCoordinates(IEnumerable<(int RowOffset, int ColumnOffset)> steps)
    {
        (int Row, int Column) current = (0,0);

        foreach (var step in steps)
        {
            current = (current.Row + step.RowOffset, current.Column + step.ColumnOffset);

            yield return current;
        }
    }
    
    private static IEnumerable<(int RowOffset, int ColumnOffset)> ParseInput1(string[] input) =>
        input.Select(x => x.Split())
            .Select(parts => (ParseDirection(parts[0]), int.Parse(parts[1])))
            .Select(s => (s.Item1.RowOffset * s.Item2, s.Item1.ColumnOffset * s.Item2));
    
    private static IEnumerable<(int RowOffset, int ColumnOffset)> ParseInput2(string[] input) =>
        input.Select(x => x.Split()[2][2..^1])
            .Select(hex => (ParseDirection(hex[^1]),  Convert.ToInt32(hex[..^1], 16)))
            .Select(s => (s.Item1.RowOffset * s.Item2, s.Item1.ColumnOffset * s.Item2));

    private static (int RowOffset, int ColumnOffset) ParseDirection(char input) => input switch
    {
        '3' => new(-1, 0),
        '2' => new(0, -1),
        '1' => new(1, 0),
        '0' => new(0, 1)
    };

    private static (int RowOffset, int ColumnOffset) ParseDirection(string input) => input switch
    {
        "U" => new(-1, 0),
        "L" => new(0, -1),
        "D" => new(1, 0),
        "R" => new(0, 1)
    };
}