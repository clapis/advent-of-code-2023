namespace AdventOfCode._17;

public class ClumsyCrucible
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./17/input.txt");

        var map = ParseInput(input);
        
        var result = FindMinHeatLoss(map);

        Console.WriteLine(result);
    }

    private static int FindMinHeatLoss(int[][] map)
    {
        var visited = new HashSet<State>();
        var unvisited = new PriorityQueue<State, int>();
        
        var source = new Position(0, 0);
        var target = new Position(map.Length - 1, map[0].Length - 1);

        unvisited.Enqueue(new (source, new(Direction.Right, 0)), 0);
        unvisited.Enqueue(new (source, new(Direction.Down, 0)), 0);

        while (unvisited.TryDequeue(out var state, out var heatloss))
        {
            if (visited.Contains(state))
                continue;

            visited.Add(state);

            var (current, history) = state;
            
            if (current == target)
                return heatloss;

            var allowed = GetAllowedDirections(history);

            foreach (var direction in allowed)
            {
                var neighbour = current + direction;
                
                if (!IsWithinMapBoundaries(map, neighbour))
                    continue;

                var n_heatlost = heatloss + map[neighbour.Row][neighbour.Column];
                var n_history = new History(direction, direction != history.Direction ? 1 : history.Count + 1);

                unvisited.Enqueue(new (neighbour, n_history), n_heatlost);
            }
        }

        throw new Exception("couldn't find path");
    }

    private static IEnumerable<Direction> GetAllowedDirections(History history)
    {
        if (history.Count < 10)
            yield return history.Direction;

        if (history.Count >= 4)
        {
            yield return history.Direction.TurnLeft();
            yield return history.Direction.TurnRight();
        }
    }

    private static bool IsWithinMapBoundaries(int[][] map, Position value)
        => value.Row >= 0 && value.Row < map.Length &&
           value.Column >= 0 && value.Column < map[0].Length;

    private static int[][] ParseInput(string[] input)
        => input.Select(x => x.Select(c => c - '0').ToArray()).ToArray();

    private record struct State(Position Position, History History);

    private record struct History(Direction Direction, int Count);
    
    private record struct Position(int Row, int Column)
    {
        public static Position operator +(Position a, Direction d)
            => new(a.Row + d.RowDelta, a.Column + d.ColumnDelta);
    }

    private record struct Direction(int RowDelta, int ColumnDelta)
    {
        public static readonly Direction Down = new(1, 0);
        public static readonly Direction Right = new(0, 1);

        public Direction TurnLeft() => new(-ColumnDelta, RowDelta);
        public Direction TurnRight() => new(ColumnDelta, -RowDelta);
    }
}