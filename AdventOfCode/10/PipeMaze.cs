namespace AdventOfCode._10;

public class PipeMaze
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./10/input.txt");

        var maze = input.Select(x => x.ToCharArray()).ToArray();

        var start = FindStartPosition(maze);
        var loop = TraverseLoop(maze, start);

        var tiles = CalculateTilesWithinLoop(loop);
        
        Console.WriteLine(tiles);
    }
    
    private static int CalculateTilesWithinLoop(Position[] positions)
    {
        // https://en.wikipedia.org/wiki/Shoelace_formula
        // Determine the area of a polygon by its vertices 
        var area = 0;
        
        for (var i = 0; i < positions.Length; i++)
            area += positions[(i + 1) % positions.Length].Column * positions[i].Row -
                    positions[(i + 1) % positions.Length].Row * positions[i].Column;

        area = Math.Abs(area / 2);
        
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        // Determine number of points interior to the polygon
        return area + 1 - positions.Length / 2;
    }

    private static Position[] TraverseLoop(char[][] maze, Position start)
    {
        var loop = new List<Position> { start };
        
        var current = GetConnectingPipes(maze, start).First();

        while (current != start)
        {
            var next = GetConnectingPipes(maze, current)
                .Single(x => x != loop.Last());
            
            loop.Add(current);

            current = next;
        }

        return loop.ToArray();
    }

    private static Position FindStartPosition(char[][] maze)
    {
        for (int i = 0; i < maze.Length; i++)
        for (int j = 0; j < maze[i].Length; j++)
            if (maze[i][j] == 'S') return new Position(i, j);

        throw new Exception("Cannot find start");
    }
    
    private static readonly char[] UpSymbols = { '|', 'J', 'L', 'S' };
    private static readonly char[] DownSymbols = { '|', 'F', '7', 'S' };
    private static readonly char[] LeftSymbols = { '-', '7', 'J', 'S' };
    private static readonly char[] RightSymbols = { '-', 'F', 'L', 'S' };

    private static IEnumerable<Position> GetConnectingPipes(char[][] maze, Position start)
    {
        var symbol = maze[start.Row][start.Column];
        
        if (UpSymbols.Contains(symbol) && 
            start.Row > 0 && DownSymbols.Contains(maze[start.Row - 1][start.Column]))
            yield return start with { Row = start.Row - 1 };

        if (DownSymbols.Contains(symbol) && 
            start.Row < maze.Length - 1 && UpSymbols.Contains(maze[start.Row + 1][start.Column]))
            yield return start with { Row = start.Row + 1 };

        if (LeftSymbols.Contains(symbol) && 
            start.Column > 0 && RightSymbols.Contains(maze[start.Row][start.Column - 1]))
            yield return start with { Column = start.Column - 1 };

        if (RightSymbols.Contains(symbol) && 
            start.Column < maze[start.Row].Length - 1 && LeftSymbols.Contains(maze[start.Row][start.Column + 1]))
            yield return start with { Column = start.Column + 1 };
    }

    private readonly record struct Position(int Row, int Column);
}