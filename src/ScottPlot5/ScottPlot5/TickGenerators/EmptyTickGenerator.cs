namespace ScottPlot.TickGenerators;

public class EmptyTickGenerator : ITickGenerator
{
    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 50;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
    }
}
