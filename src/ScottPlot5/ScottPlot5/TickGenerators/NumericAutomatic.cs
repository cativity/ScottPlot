namespace ScottPlot.TickGenerators;

public class NumericAutomatic : ITickGenerator
{
    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount
    {
        get => TickSpacingCalculator.MaximumTickCount;
        set => TickSpacingCalculator.MaximumTickCount = value;
    }

    public bool IntegerTicksOnly { get; set; }

    public Func<double, string> LabelFormatter { get; set; } = LabelFormatters.Numeric;

    public IMinorTickGenerator MinorTickGenerator { get; set; } = new EvenlySpacedMinorTickGenerator(5);

    public readonly DecimalTickSpacingCalculator TickSpacingCalculator = new DecimalTickSpacingCalculator();

    public float MinimumTickSpacing { get; set; }

    public double TickDensity { get; set; } = 1.0; // TODO: consider adding logic to make this a fraction of the width in pixels

    public int? TargetTickCount;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        Ticks = GenerateTicks(range, edge, size, new PixelLength(12), paint, labelStyle).Where(x => range.Contains(x.Position)).ToArray();
    }

    private Tick[] GenerateTicks(CoordinateRange range, Edge edge, PixelLength axisLength, PixelLength maxLabelLength, SKPaint paint, LabelStyle labelStyle, int depth = 0)
    {
        while (true)
        {
            if (depth > 3)
            {
                Debug.WriteLine($"Warning: Tick recursion depth = {depth}");
            }

            // generate ticks and labels based on predicted maximum label size
            float labelWidth = Math.Max(MinimumTickSpacing, maxLabelLength.Length * (1 / (float)TickDensity));

            if (TargetTickCount.HasValue)
            {
                labelWidth = axisLength.Length / (TargetTickCount.Value + 1);
            }

            double[] majorTickPositions = TickSpacingCalculator.GenerateTickPositions(range, axisLength, labelWidth);
            string[] majorTickLabels = majorTickPositions.Select(x => LabelFormatter(x)).ToArray();

            // determine if the actual tick labels are larger than predicted,
            // suggesting density is too high and overlapping may occur.
            (string largestText, PixelLength actualMaxLength) = edge.IsVertical() ? labelStyle.MeasureHighestString(majorTickLabels, paint) : labelStyle.MeasureWidestString(majorTickLabels, paint);

            // recursively recalculate tick density if necessary
            if (actualMaxLength.Length > maxLabelLength.Length)
            {
                maxLabelLength = actualMaxLength;
                depth++;

                continue;
            }

            return GenerateFinalTicks(majorTickPositions, majorTickLabels, range);
        }
    }

    private Tick[] GenerateFinalTicks(double[] positions, string[] labels, CoordinateRange visibleRange)
    {
        if (IntegerTicksOnly)
        {
            List<int> indexesToKeep = [];

            for (int i = 0; i < positions.Length; i++)
            {
                double position = positions[i];
                double distanceFromInteger = Math.Abs(position - (int)position);

                if (distanceFromInteger < .01)
                {
                    indexesToKeep.Add(i);
                }
            }

            positions = indexesToKeep.Select(x => positions[x]).ToArray();
            labels = indexesToKeep.Select(x => labels[x]).ToArray();
        }

        IEnumerable<Tick> majorTicks = positions.Select((position, i) => Tick.Major(position, labels[i]));

        IEnumerable<Tick> minorTicks = MinorTickGenerator.GetMinorTicks(positions, visibleRange).Select(Tick.Minor);

        return [.. majorTicks, .. minorTicks];
    }
}
