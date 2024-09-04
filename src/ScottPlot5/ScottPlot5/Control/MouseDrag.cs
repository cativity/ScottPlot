namespace ScottPlot.Control;

public readonly struct MouseDrag(MultiAxisLimitManager limits, Pixel from, Pixel to)
{
    public readonly MultiAxisLimitManager InitialLimits = limits;
    public readonly Pixel From = from;
    public readonly Pixel To = to;
}
