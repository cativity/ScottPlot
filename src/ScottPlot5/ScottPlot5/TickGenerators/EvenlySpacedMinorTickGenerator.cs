namespace ScottPlot.TickGenerators;

public class EvenlySpacedMinorTickGenerator(double minorTickPerMajorTick) : IMinorTickGenerator
{
    public double MinorTicksPerMajorTick { get; set; } = minorTickPerMajorTick;

    public List<double> GetMinorTicks(double[] majorTicks, CoordinateRange visibleRange)
    {
        if (majorTicks.Length < 2)
        {
            return [];
        }

        double majorTickSpacing = majorTicks[1] - majorTicks[0];
        double minorTickSpacing = majorTickSpacing / MinorTicksPerMajorTick;

        List<double> majorTicksWithPadding = [majorTicks[0] - majorTickSpacing, .. majorTicks];

        List<double> minorTicks = [];

        foreach (double majorTickPosition in majorTicksWithPadding)
        {
            for (int i = 1; i < MinorTicksPerMajorTick; i++)
            {
                double minorTickPosition = majorTickPosition + (minorTickSpacing * i);

                if (visibleRange.Contains(minorTickPosition))
                {
                    minorTicks.Add(minorTickPosition);
                }
            }
        }

        return minorTicks;
    }
}
