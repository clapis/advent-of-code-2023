using System.Text.RegularExpressions;

namespace AdventOfCode._19;

public class Aplenty
{
    public static async Task MainAsync()
    {
        var input = await File.ReadAllTextAsync("./19/input.txt");

        var blocks = input.Split("\n\n");

        var workflows = ParseWorkflows(blocks[0]);

        var paths = GetAcceptablePaths(workflows);

        var result = paths.Sum(rules =>
        {
            var range = "xmas"
                .ToDictionary(key => $"{key}", _ => new Range(1, 4000));

            foreach (var rule in rules) Apply(range, rule);

            return range.Values.Aggregate(1L, (acc, ran) => acc * (ran.End - ran.Start + 1));
        });

        Console.WriteLine(result);
    }

    private static IEnumerable<List<Rule>> GetAcceptablePaths(Dictionary<string, Workflow> workflows)
    {
        var queue = new Queue<(string Name, List<Rule> Rules)>();
        
        queue.Enqueue(("in", new()));

        while (queue.Any())
        {
            var (name, rules) = queue.Dequeue();

            if (name is "A") yield return rules;
            
            if (!workflows.ContainsKey(name)) continue;

            var workflow = workflows[name];

            var negatives = new List<Rule>();

            foreach (var rule in workflow.Rules)
            {
                queue.Enqueue((rule.Next, rules.Union(negatives).Append(rule).ToList()));
                
                negatives.Add(Negate(rule));
            }
            
            queue.Enqueue((workflow.Fallback, rules.Union(negatives).ToList()));
        }
    }
    
    private static Rule Negate(Rule input)
        => input.Symbol == ">" ? 
            input with { Symbol = "<", Value = input.Value + 1 } : 
            input with { Symbol = ">", Value = input.Value - 1 };

    private static void Apply(Dictionary<string, Range> ranges, Rule rule)
    {
        if (rule.Symbol == ">" && ranges[rule.Letter].Start <= rule.Value)
            ranges[rule.Letter] = ranges[rule.Letter] with { Start = rule.Value + 1 };

        if (rule.Symbol == "<" && ranges[rule.Letter].End >= rule.Value)
            ranges[rule.Letter] = ranges[rule.Letter] with { End = rule.Value - 1 };

        if (ranges[rule.Letter].End < ranges[rule.Letter].Start)
            throw new Exception("assert false");
    }
    
    private static Dictionary<string, Workflow> ParseWorkflows(string block)
    {
        return block.Split("\n")
            .Select(line => line.Split(new [] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(blocks =>
            {
                var name = blocks[0];
                var spec = blocks[1].Split(",");
                var rules = spec[..^1].Select(ParseRule).ToArray();
                var fallback = spec[^1];
                
                return new Workflow(name, rules, fallback);
            })
            .ToDictionary(x => x.Name);
    }

    private static Rule ParseRule(string input)
    {
        var match = Regex.Match(input, @"(?<letter>[xmas])(?<symbol>[<>])(?<value>\d+):(?<next>\w+)");
        
        return new Rule(match.Groups["letter"].Value, match.Groups["symbol"].Value, 
            int.Parse(match.Groups["value"].Value), match.Groups["next"].Value);
    }
    
    private record Range(int Start, int End);
    private record Workflow(string Name, Rule[] Rules, string Fallback);
    private record Rule(string Letter, string Symbol, int Value, string Next);
}