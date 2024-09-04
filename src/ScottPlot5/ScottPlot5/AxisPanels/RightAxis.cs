using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class RightAxis : YAxisBase, IYAxis
{
    public override Edge Edge => Edge.Right;

    public RightAxis()
    {
        TickGenerator = new NumericAutomatic();
        LabelRotation = 90;
    }
}
