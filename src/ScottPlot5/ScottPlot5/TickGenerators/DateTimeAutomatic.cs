using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.TickGenerators;

public class DateTimeAutomatic : IDateTimeTickGenerator
{
    /// <summary>
    ///     If assigned, this function will be used to create tick labels
    /// </summary>
    public Func<DateTime, string>? LabelFormatter { get; set; }

    public ITimeUnit? TimeUnit { get; private set; }

    private static readonly List<ITimeUnit> _theseTimeUnits =
    [
        new Millisecond(),
        new Centisecond(),
        new Decisecond(),
        new Second(),
        new Minute(),
        new Hour(),
        new Day(),
        new Month(),
        new Year(),
    ];

    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 10_000;

    private static ITimeUnit GetAppropriateTimeUnit(TimeSpan timeSpan, int targetTickCount = 10)
    {
        foreach (ITimeUnit? timeUnit in _theseTimeUnits)
        {
            long estimatedUnitTicks = timeSpan.Ticks / timeUnit.MinSize.Ticks;

            if (timeUnit.Divisors.Select(increment => estimatedUnitTicks / increment).Any(estimatedTicks => estimatedTicks > targetTickCount / 3 && estimatedTicks < targetTickCount * 3))
            {
                return timeUnit;
            }
        }

        return _theseTimeUnits[^1];
    }

    private static ITimeUnit GetLargerTimeUnit(ITimeUnit timeUnit)
    {
        for (int i = 0; i < _theseTimeUnits.Count - 1; i++)
        {
            if (timeUnit.GetType() == _theseTimeUnits[i].GetType())
            {
                return _theseTimeUnits[i + 1];
            }
        }

        return _theseTimeUnits[^1];
    }

    private static int? LeastMemberGreaterThan(double value, IReadOnlyList<int> list)
    {
        //return list.FirstOrDefault(item => item > value);

        foreach (int item in list.Where(item => item > value))
        {
            return item;
        }

        return null;
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        if (range.Span >= TimeSpan.MaxValue.Days || double.IsNaN(range.Span) || double.IsInfinity(range.Span))
        {
            // cases of extreme zoom (10,000 years)
            Ticks = [];

            return;
        }

        TimeSpan span = TimeSpan.FromDays(range.Span);
        ITimeUnit? timeUnit = GetAppropriateTimeUnit(span);

        // estimate the size of the largest tick label for this unit this unit
        int maxExpectedTickLabelWidth = (int)Math.Max(16, span.TotalDays / MaxTickCount);
        const int tickLabelHeight = 12;
        PixelSize tickLabelBounds = new PixelSize(maxExpectedTickLabelWidth, tickLabelHeight);
        double coordinatesPerPixel = range.Span / size.Length;

        while (true)
        {
            // determine the ideal spacing to use between ticks
            double increment = coordinatesPerPixel * tickLabelBounds.Width / timeUnit.MinSize.TotalDays;
            int? niceIncrement = LeastMemberGreaterThan(increment, timeUnit.Divisors);

            if (niceIncrement is null)
            {
                timeUnit = _theseTimeUnits.Find(t => t.MinSize > timeUnit.MinSize);

                if (timeUnit is not null)
                {
                    continue;
                }

                timeUnit = _theseTimeUnits[^1];
                niceIncrement = (int)Math.Ceiling(increment);
            }

            TimeUnit = timeUnit;

            // attempt to generate the ticks given these conditions
            (List<Tick>? ticks, PixelSize? largestTickLabelSize) = GenerateTicks(range, timeUnit, niceIncrement.Value, tickLabelBounds, paint, labelStyle);

            // if ticks were returned, use them
            if (ticks is not null)
            {
                Ticks = [.. ticks];

                return;
            }

            // if no ticks were returned it means the conditions were too dense and tick labels
            // overlapped, so expand the tick label bounds and try again.
            if (largestTickLabelSize is not null)
            {
                tickLabelBounds = tickLabelBounds.Max(largestTickLabelSize.Value);
                tickLabelBounds = new PixelSize(tickLabelBounds.Width + 10, tickLabelBounds.Height + 10);

                continue;
            }

            throw new InvalidOperationException($"{nameof(ticks)} and {nameof(largestTickLabelSize)} are both null");
        }
    }

    /// <summary>
    ///     This method attempts to find an ideal set of ticks.
    ///     If all labels fit within the bounds, the list of ticks is returned.
    ///     If a label doesn't fit in the bounds, the list is null and the size of the large tick label is returned.
    /// </summary>
    private (List<Tick>? Positions, PixelSize? PixelSize) GenerateTicks(CoordinateRange range,
                                                                        ITimeUnit unit,
                                                                        int increment,
                                                                        PixelSize tickLabelBounds,
                                                                        SKPaint paint,
                                                                        LabelStyle labelStyle)
    {
        DateTime rangeMin = NumericConversion.ToDateTime(range.Min);
        DateTime rangeMax = NumericConversion.ToDateTime(range.Max);

        // range.Min could be anything, but when calculating start it must be "snapped" to the best tick
        DateTime start = GetLargerTimeUnit(unit).Snap(rangeMin);

        start = unit.Next(start, -increment);

        List<Tick> ticks = [];

        const int maxTickCount = 1000;

        for (DateTime dt = start; dt <= rangeMax; dt = unit.Next(dt, increment))
        {
            if (dt < rangeMin)
            {
                continue;
            }

            string tickLabel = LabelFormatter is null ? dt.ToString(unit.GetDateTimeFormatString()) : LabelFormatter(dt);

            PixelSize tickLabelSize = labelStyle.Measure(tickLabel, paint).Size;

            bool tickLabelIsTooLarge = !tickLabelBounds.Contains(tickLabelSize);

            if (tickLabelIsTooLarge)
            {
                return (null, tickLabelSize);
            }

            double tickPosition = NumericConversion.ToNumber(dt);
            Tick tick = new Tick(tickPosition, tickLabel, true);
            ticks.Add(tick);

            // this prevents infinite loops with weird axis limits or small delta (e.g., DateTime)
            if (ticks.Count >= maxTickCount)
            {
                break;
            }
        }

        return (ticks, null);
    }

    public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) => dates.Select(NumericConversion.ToNumber);
}
