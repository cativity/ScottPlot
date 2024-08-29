namespace ScottPlot.TickGenerators;

public class LogMinorTickGenerator : IMinorTickGenerator
{
    public int Divisions { get; set; } = 5;

    public List<double> GetMinorTicks(double[] majorPositions, CoordinateRange visibleRange)
    {
        if (majorPositions.Length < 2)
        {
            return [];
        }

        // determine range of major ticks to iterate (assume even spacing of all ticks)
        double deltaMajor = majorPositions[1] - majorPositions[0];
        double lowestMajor = majorPositions[0] - deltaMajor;
        double highestMajor = majorPositions[^1] + deltaMajor;

        // pre-calculate the log-distributed offset positions between major ticks
        List<double> minorTickOffsets = Enumerable.Range(1, Divisions - 1).Select(x => deltaMajor * Math.Log10(x * 10D / Divisions)).ToList();

        // iterate major ticks and collect minor ticks with offsets
        List<double> minorTicks = [];

        for (double major = lowestMajor; major <= highestMajor; major += deltaMajor)
        {
            minorTicks.AddRange(minorTickOffsets.Select(offset => major + offset));
        }

        return minorTicks;
    }
}
