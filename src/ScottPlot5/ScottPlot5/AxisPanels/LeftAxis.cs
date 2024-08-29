using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class LeftAxis : YAxisBase, IYAxis
{
    public override Edge Edge { get; } = Edge.Left;

    public LeftAxis()
    {
        TickGenerator = new NumericAutomatic();
        LabelRotation = -90;
    }
}
