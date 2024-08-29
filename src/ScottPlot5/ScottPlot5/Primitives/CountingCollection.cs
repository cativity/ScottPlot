namespace ScottPlot;

public class CountingCollection<T>
    where T : notnull
{
    public readonly Dictionary<T, int> Counts = [];

    public bool Any() => Counts.Count != 0;

    public int Count => Counts.Count;

    public IEnumerable<T> SortedKeys => Counts.OrderBy(static x => x.Value).Select(static x => x.Key);

    public void Add(T item)
    {
        if (!Counts.TryAdd(item, 1))
        {
            Counts[item]++;
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Add(item);
        }
    }

    public override string ToString() => $"CountingCollection<{typeof(T)}> with {Count} items";

    public string GetLongString()
    {
        return string.Join(", ", SortedKeys.Select(x => $"{x} ({Counts[x]})"));
    }
}
