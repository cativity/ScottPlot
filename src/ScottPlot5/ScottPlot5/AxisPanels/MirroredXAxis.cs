namespace ScottPlot.AxisPanels;

public sealed class MirroredXAxis : XAxisBase, IXAxis
{
    private readonly IXAxis _axis;

    public override Edge Edge { get; }

    public override CoordinateRangeMutable Range => new CoordinateRangeMutable(_axis.Min, _axis.Max);

    public MirroredXAxis(IXAxis axis, Edge? edge)
    {
        _axis = axis;
        Edge = edge ?? axis.Edge;
        TickGenerator = axis.TickGenerator;
    }
}
