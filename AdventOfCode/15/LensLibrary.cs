
namespace AdventOfCode._15;

public class LensLibrary
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllTextAsync("./15/input.txt");

        var boxes = Enumerable.Range(0, 256)
            .Select(_ => new List<Step>())
            .ToArray();
        
        foreach (var code in input.Split(","))
        {
            var label = code.Split('=', '-')[0];
            var focal = code[(label.Length + 1)..];

            var step = new Step(label, focal);
            var box = boxes[Hash(label)];

            var index = box.FindIndex(x => x.Label == label);

            if (code[label.Length] == '-' && index >= 0)
                box.RemoveAt(index);
            else if (code[label.Length] == '=' && index >= 0)
                box[index] = step;
            else if (code[label.Length] == '=')
                box.Add(step);
        }

        var total = Enumerable.Range(0, boxes.Length)
            .SelectMany(ibox => Enumerable.Range(0, boxes[ibox].Count)
                .Select(ilens => (ibox + 1) * (ilens + 1) * int.Parse(boxes[ibox][ilens].Focal)))
            .Sum();

        Console.WriteLine(total);
    }

    private static int Hash(string input) 
        => input.Aggregate(0, (total, ascii) => (total + ascii) * 17 % 256);

    private record Step(string Label, string Focal);
}