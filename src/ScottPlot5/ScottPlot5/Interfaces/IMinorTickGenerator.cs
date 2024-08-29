namespace ScottPlot;

public interface IMinorTickGenerator
{
    List<double> GetMinorTicks(double[] majorTicks, CoordinateRange visibleRange);
}
