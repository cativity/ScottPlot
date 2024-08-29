namespace ScottPlot.AxisPanels;

public sealed class MirroredYAxis : YAxisBase, IYAxis
{
    private readonly IYAxis _axis;

    public override Edge Edge { get; }

    public override CoordinateRangeMutable Range => new CoordinateRangeMutable(_axis.Min, _axis.Max);

    public MirroredYAxis(IYAxis axis, Edge? edge)
    {
        _axis = axis;
        Edge = edge ?? axis.Edge;
        TickGenerator = axis.TickGenerator;
    }
}
