namespace ScottPlot.DataSources;

// ReSharper disable once InconsistentNaming
public class OHLCSource(List<OHLC> prices) : IOHLCSource
{
    public List<OHLC> GetOHLCs() => prices;

    public AxisLimits GetLimits() => prices.Count != 0 ? new AxisLimits(GetLimitsX(), GetLimitsY()) : AxisLimits.NoLimits;

    public CoordinateRange GetLimitsX()
    {
        List<DateTime> dates = prices.ConvertAll(static x => x.DateTime);

        return new CoordinateRange(NumericConversion.ToNumber(dates.Min()), NumericConversion.ToNumber(dates.Max()));
    }

    public CoordinateRange GetLimitsY()
    {
        List<CoordinateRange> priceRanges = prices.ConvertAll(static x => x.GetPriceRange());
        double min = priceRanges.Min(static x => x.Min);
        double max = priceRanges.Max(static x => x.Max);

        return new CoordinateRange(min, max);
    }
}
