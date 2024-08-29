using ScottPlot.Statistics;

namespace ScottPlot.Finance;

public class SimpleMovingAverage
{
    public readonly double[] Means;
    public readonly DateTime[] DateTimes;
    public readonly double[] Dates;

    public SimpleMovingAverage(List<OHLC> ohlcs, int n)
    {
        double[] prices = ohlcs.Select(static x => x.Close).ToArray();
        Means = Series.MovingAverage(prices, n);
        DateTimes = ohlcs.Skip(n).Select(static x => x.DateTime).ToArray();
        Dates = DateTimes.Select(static x => x.ToOADate()).ToArray();
    }
}
