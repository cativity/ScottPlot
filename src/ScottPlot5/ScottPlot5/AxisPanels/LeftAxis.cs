using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class LeftAxis : YAxisBase, IYAxis
{
    public override Edge Edge => Edge.Left;

    public LeftAxis()
    {
        TickGenerator = new NumericAutomatic();
        LabelRotation = -90;
    }
}
