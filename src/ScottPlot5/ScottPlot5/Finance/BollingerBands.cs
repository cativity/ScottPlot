using ScottPlot.Statistics;

namespace ScottPlot.Finance;

public class BollingerBands
{
    public readonly double[] Means;
    public readonly double[] UpperValues;
    public readonly double[] LowerValues;
    public readonly DateTime[] DateTimes;
    public readonly double[] Dates;

    public BollingerBands(List<OHLC> ohlcs, int n, double sdCoeff = 2)
    {
        double[] prices = ohlcs.Select(static x => x.Close).ToArray();
        double[] sma = Series.MovingAverage(prices, n, true);
        double[] smstd = Series.SimpleMovingStandardDeviation(prices, n, true);

        UpperValues = new double[prices.Length];
        LowerValues = new double[prices.Length];

        for (int i = 0; i < prices.Length; i++)
        {
            LowerValues[i] = sma[i] - (sdCoeff * smstd[i]);
            UpperValues[i] = sma[i] + (sdCoeff * smstd[i]);
        }

        // skip the first points which all contain NaN
        Means = sma.Skip(n).ToArray();
        LowerValues = LowerValues.Skip(n).ToArray();
        UpperValues = UpperValues.Skip(n).ToArray();
        DateTimes = ohlcs.Skip(n).Select(static x => x.DateTime).ToArray();
        Dates = DateTimes.Select(static x => x.ToOADate()).ToArray();
    }
}
