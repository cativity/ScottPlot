using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class RightAxis : YAxisBase, IYAxis
{
    public override Edge Edge { get; } = Edge.Right;

    public RightAxis()
    {
        TickGenerator = new NumericAutomatic();
        LabelRotation = 90;
    }
}
