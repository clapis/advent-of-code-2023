namespace AdventOfCode._05;

public record Almanac(
    List<Range> Seeds, 
    List<AlmanacMap> Maps);

public record AlmanacMap(List<AlmanacMapRule> Rules);

public record AlmanacMapRule(Range Source, Range Destination);

public record Range(long Start, long End)
{
    public bool Intersects(Range input)
        => input.End >= Start && input.Start <= End;

    public Range Shift(long offset)
        => new Range(Start + offset, End + offset);

    public Range Intersection(Range input)
    {
        if (!Intersects(input))
            throw new Exception("Doesn't intersect!");
                
        var start = Math.Max(Start, input.Start);
        var end = Math.Min(End, input.End);

        return new Range(start, end);
    }
    
    public IEnumerable<Range> Except(Range input)
    {
        if (!input.Intersects(this))
            yield return new Range(Start, End);
            
        else
        {
            if (input.Start > Start)
                yield return this with { End = input.Start };

            if (input.End < End)
                yield return this with { Start = input.End };
        }
    }
}