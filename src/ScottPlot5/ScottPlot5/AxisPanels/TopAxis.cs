using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public class TopAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Top;

    public TopAxis() => TickGenerator = new NumericAutomatic();
}
