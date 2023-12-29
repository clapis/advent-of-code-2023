namespace AdventOfCode._21;
using Coordinates = (int Row, int Column);

public class StepCounter
{
    private static readonly (int Rx, int Cx)[] Directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    
    public static async Task RunAsync()
    {
        var input = await File.ReadAllLinesAsync("./21/input.txt");

        var start = Find(input, 'S');

        // n = 65 + 131 * k
        var steps = 26501365;
        var size = input.Length; 
        var k = (steps - size/2) / size;
        
        var values = Enumerable.Range(0, 3)
            .Select(k => size/2 + size * k)
            .Select(n => Walk(input, start, n))
            .ToList();
        
        var d1 = values.Zip(values.Skip(1)).Select(x => x.Second - x.First).ToList();
        var d2 = d1.Zip(d1.Skip(1)).Select(x => x.Second - x.First).Single();

        long total = values[1];
        long subtotal = d1[0];
        for (int i = 0; i < k - 1; i++)
        {
            subtotal += d2;
            total += subtotal;
        }
        
        Console.WriteLine(total);
    }

    private static int Walk(string[] input, Coordinates start, int count)
    {
        var result = new HashSet<Coordinates>();
        var visited = new HashSet<Coordinates>();

        var queue = new Queue<(Coordinates, int Steps)>();
        queue.Enqueue((start, count));
        
        while (queue.Any())
        {
            var (current, steps) = queue.Dequeue();

            if (steps % 2 == 0)
                result.Add(current);
            
            if (steps == 0)
                continue;

            foreach (var direction in Directions)
            {
                var neighbour = (current.Row + direction.Rx, current.Column + direction.Cx);

                if (!IsRock(neighbour, input) && visited.Add(neighbour))
                    queue.Enqueue((neighbour, steps - 1));
            }
        }
        
        return result.Count;
    }

    private static bool IsRock(Coordinates coordinates, string[] input)
    {
        var row = (input.Length + coordinates.Row % input.Length) % input.Length;
        var column = (input[row].Length + coordinates.Column % input[row].Length) % input[row].Length;

        return input[row][column] is '#';
    }

    private static Coordinates Find(string[] input, char symbol)
    {
        for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < input[i].Length; j++)
                if (input[i][j] == symbol)
                    return (i, j);

        throw new Exception($"{symbol} not found");
    }
}