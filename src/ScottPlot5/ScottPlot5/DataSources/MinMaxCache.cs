using System.Threading.Tasks;
using ScottPlot.DataSources;

namespace ScottPlot;

public class MinMaxCache
{
    private readonly IReadOnlyList<double> _data;
    private readonly IReadOnlyList<SignalRangeY> _cache;

    public int CachePeriod { get; }

    public MinMaxCache(IReadOnlyList<double> data, int cachePeriod = 1000)
    {
        _data = data;
        CachePeriod = cachePeriod;
        int cacheSize = data.Count / cachePeriod;
        SignalRangeY[] cache = new SignalRangeY[cacheSize];
        _cache = cache;

        // Create MinMax caches in parallel at regular intervals
        Parallel.For(0,
                     cacheSize,
                     i =>
                     {
                         double min = double.PositiveInfinity;
                         double max = double.NegativeInfinity;

                         for (int j = 0; j < cachePeriod; j++)
                         {
                             double sample = data[(i * cachePeriod) + j];

                             if (sample < min)
                             {
                                 min = sample;
                             }

                             if (max < sample)
                             {
                                 max = sample;
                             }
                         }

                         cache[i] = new SignalRangeY(min, max);
                     });
    }

    public SignalRangeY GetMinMax(int start, int end)
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        // Calculate Start Index Alignment Offset
        int periodStartIndex = (start / CachePeriod) + 1;
        int startOffsetCount = periodStartIndex * CachePeriod;

        double sample;

        if (end - start < CachePeriod || startOffsetCount >= end)
        {
            // If the requested index size is less than the cache period
            for (int i = start; i < end; i++)
            {
                sample = _data[i];

                if (sample < min)
                {
                    min = sample;
                }

                if (max < sample)
                {
                    max = sample;
                }
            }

            return new SignalRangeY(min, max);
        }

        // Calculate End Index Alignment Offset
        int periodEndIndex = end / CachePeriod;
        int endOffsetIndex = periodEndIndex * CachePeriod;

        // Start ~ Period Start Index
        for (int i = start; i < startOffsetCount; i++)
        {
            sample = _data[i];

            if (sample < min)
            {
                min = sample;
            }

            if (max < sample)
            {
                max = sample;
            }
        }

        // Period Start Index ~ Period End Index
        for (int i = periodStartIndex; i < periodEndIndex; i++)
        {
            SignalRangeY minMaxSample = _cache[i];

            if (minMaxSample.Min < min)
            {
                min = minMaxSample.Min;
            }

            if (max < minMaxSample.Max)
            {
                max = minMaxSample.Max;
            }
        }

        // Period End Index ~ End
        for (int i = endOffsetIndex; i < end; i++)
        {
            sample = _data[i];

            if (sample < min)
            {
                min = sample;
            }

            if (max < sample)
            {
                max = sample;
            }
        }

        return new SignalRangeY(min, max);
    }
}
