using ScottPlot.Statistics;

namespace ScottPlot;

public class Population
{
    public IReadOnlyList<double> Values { get; }

    public IReadOnlyList<double> SortedValues { get; }

    public int Count { get; }

    public double Min { get; }

    public double Max { get; }

    public double Mean { get; }

    public double Variance { get; }

    public double StandardDeviation { get; }

    public double StandardError { get; }

    public double Median { get; }

    public Population(double[] values)
    {
        Values = [.. values];
        SortedValues = [.. Values.OrderBy(static x => x)];
        Count = values.Length;
        Min = values.Min();
        Max = values.Max();
        Mean = Descriptive.Mean(values);
        Variance = Descriptive.Variance(values);
        StandardDeviation = Descriptive.StandardDeviation(values);
        StandardError = Descriptive.StandardError(values);
        Median = Descriptive.SortedMedian(SortedValues);
    }

    public double GetPercentile(double percentile) => Descriptive.SortedPercentile(SortedValues, percentile);
}
