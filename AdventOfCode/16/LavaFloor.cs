namespace AdventOfCode._16;

public class LavaFloor
{
    public static async Task MainAsync()
    {
        var map = await File.ReadAllLinesAsync("./16/input.txt");

        var result = GetPossibleStartSteps(map)
            .Max(s => CalculateTotalEnergizedTiles(s, map));

        Console.WriteLine(result);
    }

    private static IEnumerable<Step> GetPossibleStartSteps(string[] map)
    {
        var left = Enumerable.Range(0, map.Length - 1)
            .Select(i => new Step(i, 0, Direction.Right));
        
        var right = Enumerable.Range(0, map.Length - 1)
            .Select(i => new Step(i, map[0].Length - 1, Direction.Left));
        
        var top = Enumerable.Range(0, map[0].Length - 1)
            .Select(i => new Step(0, i, Direction.Down));
        
        var bottom = Enumerable.Range(0, map[0].Length - 1)
            .Select(i => new Step(map.Length - 1, i, Direction.Up));

        return left.Union(right).Union(top).Union(bottom);
    }

    private static int CalculateTotalEnergizedTiles(Step start, string[] map)
    {
        var visited = new HashSet<Step>();
        var unvisited = new Queue<Step>();
        
        unvisited.Enqueue(start);

        while (unvisited.Any())
        {
            var current = unvisited.Dequeue();

            var symbol = map[current.Row][current.Column];

            var discovered = GetNextSteps(symbol, current)
                .Where(s => IsWithinMap(map, s))
                .Where(s => !visited.Contains(s));
            
            foreach (var next in discovered)
                unvisited.Enqueue(next);
            
            visited.Add(current);
        }
        
        return visited.DistinctBy(step => (step.Row, step.Column)).Count();
    }

    private static IEnumerable<Step> GetNextSteps(char symbol, Step step)
        => GetNextDirections(symbol, step.Direction).Select(d => Move(step, d));

    private static IEnumerable<Direction> GetNextDirections(char symbol, Direction direction)
    {
        if (symbol == '.')
            yield return direction;
        
        if (symbol == '-')
        {
            if (direction is not Direction.Left)
                yield return Direction.Right;
            if (direction is not Direction.Right)
                yield return Direction.Left;
        }
        
        if (symbol == '|')
        {
            if (direction is not Direction.Up)
                yield return Direction.Down;
            if (direction is not Direction.Down)
                yield return Direction.Up;
        }

        if (symbol == '/')
        {
            if (direction is Direction.Right)
                yield return Direction.Up;
            if (direction is Direction.Left)
                yield return Direction.Down;
            if (direction is Direction.Up)
                yield return Direction.Right;
            if (direction is Direction.Down)
                yield return Direction.Left;
        }
        
        if (symbol == '\\')
        {
            if (direction is Direction.Right)
                yield return Direction.Down;
            if (direction is Direction.Left)
                yield return Direction.Up;
            if (direction is Direction.Up)
                yield return Direction.Left;
            if (direction is Direction.Down)
                yield return Direction.Right;
        }
    }

    private static bool IsWithinMap(string[] map, Step step) 
        => step.Row >= 0 && step.Row < map.Length && step.Column >= 0 && step.Column < map[0].Length;

    private static Step Move(Step step, Direction direction) => direction switch
        {
            Direction.Up => step with { Row = step.Row - 1, Direction = direction},
            Direction.Down => step with { Row = step.Row + 1, Direction = direction},
            Direction.Left => step with { Column = step.Column - 1, Direction = direction},
            Direction.Right => step with { Column = step.Column + 1, Direction = direction},
            _ => throw new ArgumentOutOfRangeException($"Unmmaped direction {direction}")
        };

    private enum Direction { Up, Down, Left, Right };
    private record struct Step(int Row, int Column, Direction Direction);
}