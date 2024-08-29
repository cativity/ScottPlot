namespace ScottPlot.TickGenerators;

public class DecimalTickSpacingCalculator
{
    public int MaximumTickCount { get; set; } = 1000;

    public double[] GenerateTickPositions(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        double absSpan = Math.Abs(range.Span);
        double rangeMin = Math.Min(range.Min, range.Max);
        double tickSpacing = GetIdealTickSpacing(range, axisLength, maxLabelLength);

        double firstTickOffset = rangeMin % tickSpacing;
        int tickCount = (int)(absSpan / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] majorTickPositions = Enumerable.Range(0, tickCount)
                                                .Select(x => rangeMin - firstTickOffset + (tickSpacing * x))
                                                .Where(range.Contains)
                                                .ToArray();

        if (majorTickPositions.Length >= 2)
        {
            return majorTickPositions;
        }

        double tickBelow = rangeMin - firstTickOffset;
        double firstTick = majorTickPositions.Length > 0 ? majorTickPositions[0] : tickBelow;
        double nextTick = tickBelow + tickSpacing;
        majorTickPositions = [firstTick, nextTick];

        return majorTickPositions;
    }

    private double GetIdealTickSpacing(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        double absSpan = Math.Abs(range.Span);

        int targetTickCount = (int)(axisLength.Length / maxLabelLength.Length) + 1;

        const int radix = 10;
        int exponent = (int)Math.Log(absSpan, radix) + 1;
        double initialSpace = Math.Pow(radix, exponent);
        List<double> tickSpacings = [initialSpace];

        double[] divBy = [2, 2, 2.5]; // 10, 5, 2.5, 1

        //if (radix == 10)
        //{
        //    divBy = [2, 2, 2.5]; // 10, 5, 2.5, 1
        //}
        //else if (radix == 16)
        //{
        //    divBy = [2, 2, 2, 2]; // 16, 8, 4, 2, 1
        //}
        //else
        //{
        //    throw new ArgumentException($"radix {radix} is not supported");
        //}
        // generate possible tick spacings
        while (tickSpacings.Count < MaximumTickCount)
        {
            double divisor = divBy[tickSpacings.Count % divBy.Length];
            double smallerSpacing = tickSpacings[^1] / divisor;
            tickSpacings.Add(smallerSpacing);
            int tickCount = (int)(absSpan / tickSpacings[^1]);

            if (tickCount > targetTickCount)
            {
                break;
            }
        }

        // choose the densest tick spacing that is still good
        for (int i = 0; i < tickSpacings.Count; i++)
        {
            double thisTickSpacing = tickSpacings[tickSpacings.Count - 1 - i];
            double thisTickCount = absSpan / thisTickSpacing;
            double spacePerTick = axisLength.Length / thisTickCount;
            double neededSpacePerTick = maxLabelLength.Length;

            neededSpacePerTick *= neededSpacePerTick switch
            {
                // add more space between small labels
                < 10 => 2,
                < 25 => 1.5,
                _ => 1.2
            };

            if (spacePerTick > neededSpacePerTick)
            {
                return thisTickSpacing;
            }
        }

        return tickSpacings[0];
    }
}
