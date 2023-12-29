namespace AdventOfCode._20;

public class PulsePropagation
{
    private const StringSplitOptions DefaultSplit =
        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
    
    public static async Task MainAsync()
    {
        var input = await File.ReadAllLinesAsync("./20/input.txt");

        var modules = ParseModules(input);

        var conjunction = modules.Values.Single(x => x.Connections.Contains("rx"));
        
        var result = GetSourcesCycleLength(modules, conjunction)
            .Aggregate(1L, (acc, i) => LeastCommonMultiple(acc, i));

        Console.WriteLine(result);
    }

    private static IEnumerable<int> GetSourcesCycleLength(Dictionary<string, Module> modules, Module module)
    {
        var sources = module.Sources.Keys.ToHashSet();

        var cycles = 1;
        
        while (sources.Count > 0)
        {
            var pulses = PressButton(modules).ToLookup(x => x.Source);
            
            var count = sources.RemoveWhere(source 
                => pulses[source].Any(p => p.Value == PulseValue.High));
            
            foreach(var _ in Enumerable.Range(1, count))
                yield return cycles;

            cycles++;
        }
    }

    private static IEnumerable<Pulse> PressButton(Dictionary<string, Module> modules)
    {
        var queue = new Queue<Pulse>();

        queue.Enqueue(new Pulse("button", "roadcaster", PulseValue.Low));

        while (queue.Any())
        {
            var pulse = queue.Dequeue();

            if (!modules.ContainsKey(pulse.Destination))
                continue;

            foreach (var emitted in Apply(modules[pulse.Destination], pulse)) 
                queue.Enqueue(emitted);
            
            yield return pulse;
        }
    }
    
    private static IEnumerable<Pulse> Apply(Module module, Pulse pulse)
    {
        if (module.Type == "b")
            return module.Connections.Select(d => new Pulse(module.Name, d, pulse.Value));

        if (module.Type == "%")
        {
            if (pulse.Value == PulseValue.High)
                return Enumerable.Empty<Pulse>();

            module.State = !module.State;
                
            return module.Connections.Select(d => new Pulse(module.Name, d, module.State 
                ? PulseValue.High : PulseValue.Low));
        }

        if (module.Type == "&")
        {
            module.Sources[pulse.Source] = pulse.Value;

            var emit = module.Sources.Any(x => x.Value == PulseValue.Low) 
                ? PulseValue.High : PulseValue.Low;
                
            return module.Connections.Select(d => new Pulse(module.Name, d, emit));
        }
        
        throw new Exception("assert false");
    }
    
    private static long LeastCommonMultiple (long a, long b) 
        => a * b / GreatestCommonFactor(a, b);

    private static long GreatestCommonFactor(long a, long b)
        => b == 0 ? a : GreatestCommonFactor(b, a % b);

    private static Dictionary<string, Module> ParseModules(string[] config)
    {
        var modules = config.Select(ParseModule)
            .ToDictionary(x => x.Name);

        foreach (var module in modules.Values)
            foreach (var connection in module.Connections)
                if (modules.TryGetValue(connection, out var mod))
                    mod.Sources[module.Name] = PulseValue.Low;

        return modules;
    }

    private static Module ParseModule(string spec)
    {
        var type = spec[..1];
        var parts = spec[1..].Split("->", DefaultSplit);
        var name = parts[0];
        var connections = parts[1].Split(",", DefaultSplit);

        return new Module(name, type, connections);
    }

    record Module(string Name, string Type, string[] Connections)
    {
        public bool State { get; set; }
        public Dictionary<string, PulseValue> Sources { get; } = new();
    }

    record Pulse(string Source, string Destination, PulseValue Value);
    
    enum PulseValue { Low, High }
}