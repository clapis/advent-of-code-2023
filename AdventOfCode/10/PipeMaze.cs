namespace AdventOfCode._10;

public class PipeMaze
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./10/input.txt");

        var maze = input.Select(x => x.ToCharArray()).ToArray();

        var start = FindStartPosition(maze);
        var direction = FindStartDirection(maze, start);
        var loop = TraverseLoop(maze, start, direction);

        var tiles = CalculateTilesWithinLoop(loop.ToArray());
        
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

    private static IEnumerable<Position> TraverseLoop(char[][] maze, Position start, Direction direction)
    {
        var current = start.Move(direction);

        while (current != start)
        {
            yield return current;

            direction = GetDestination(maze[current.Row][current.Column], direction);
            current = current.Move(direction);

        }
        
        yield return current;
    }

    private static Position FindStartPosition(char[][] maze)
    {
        for (int i = 0; i < maze.Length; i++)
        for (int j = 0; j < maze[i].Length; j++)
            if (maze[i][j] == 'S') return new Position(i, j);

        throw new Exception("Cannot find start");
    }
    
    private static readonly char[] UpSymbols = { '|', 'J', 'L' };
    private static readonly char[] DownSymbols = { '|', 'F', '7' };
    private static readonly char[] LeftSymbols = { '-', '7', 'J' };
    private static readonly char[] RightSymbols = { '_', 'F', 'L' };

    private static Direction FindStartDirection(char[][] maze, Position start)
    {
        if (start.Row > 0 && DownSymbols.Contains(maze[start.Row - 1][start.Column]))
            return Direction.Up;
        
        if (start.Row < maze.Length - 1 && UpSymbols.Contains(maze[start.Row + 1][start.Column]))
            return Direction.Down;

        if (start.Column > 0 && RightSymbols.Contains(maze[start.Row][start.Column - 1]))
            return Direction.Left;

        if (start.Column < maze[start.Row].Length - 1 && LeftSymbols.Contains(maze[start.Row][start.Column + 1]))
            return Direction.Right;
        
        throw new Exception("Cannot find connecting pipe for start");
    }

    private static Direction GetDestination(char symbol, Direction source) => symbol switch
    {
        '|' when source == Direction.Down => Direction.Down,
        '|' when source == Direction.Up => Direction.Up,
        '-' when source == Direction.Right => Direction.Right,
        '-' when source == Direction.Left => Direction.Left,
        'L' when source == Direction.Down => Direction.Right,
        'L' when source == Direction.Left => Direction.Up,
        '7' when source == Direction.Up => Direction.Left,
        '7' when source == Direction.Right => Direction.Down,
        'J' when source == Direction.Down => Direction.Left,
        'J' when source == Direction.Right => Direction.Up,
        'F' when source == Direction.Up => Direction.Right,
        'F' when source == Direction.Left => Direction.Down,
        _ => throw new Exception($"Cannot find destination for '{symbol}', source {source}")
    };

    readonly record struct Position(int Row, int Column)
    {
        public Position Move(Direction direction) 
            => new(Row + direction.RowOffset, Column + direction.ColumnOffset);
    }

    private record struct Direction(int RowOffset, int ColumnOffset)
    {
        public static readonly Direction Left = new (0, -1);
        public static readonly Direction Right = new (0, 1);
        public static readonly Direction Down = new (1, 0);
        public static readonly Direction Up = new (-1, 0);
    }
}