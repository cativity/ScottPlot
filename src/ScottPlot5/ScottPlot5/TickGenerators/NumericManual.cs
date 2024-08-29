namespace ScottPlot.TickGenerators;

public class NumericManual : ITickGenerator
{
    public Tick[] Ticks { get; private set; } = [];

    private readonly List<Tick> _tickList = [];

    public int MaxTickCount { get; set; } = 50;

    public NumericManual()
    {
    }

    public NumericManual(Tick[] ticks)
    {
        _tickList.AddRange(ticks);
    }

    public NumericManual(double[] positions, string[] labels)
    {
        if (positions.Length != labels.Length)
        {
            throw new ArgumentException($"{nameof(positions)} must have same length as {nameof(labels)}");
        }

        for (int i = 0; i < positions.Length; i++)
        {
            AddMajor(positions[i], labels[i]);
        }
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        Ticks = _tickList.Where(x => range.Contains(x.Position)).ToArray();
    }

    public void Add(Tick tick)
    {
        _tickList.Add(tick);
    }

    public void AddMajor(double position, string label)
    {
        Add(Tick.Major(position, label));
    }

    public void AddMinor(double position)
    {
        Add(Tick.Minor(position));
    }
}
