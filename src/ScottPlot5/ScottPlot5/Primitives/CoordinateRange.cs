namespace ScottPlot;

public readonly record struct CoordinateRange(double Min, double Max)
{
    public double Span => Max - Min;

    public double Center => (Min + Max) / 2;

    public static CoordinateRange Infinity => new CoordinateRange(double.NegativeInfinity, double.PositiveInfinity);

    public static CoordinateRange NotSet => new CoordinateRange(double.PositiveInfinity, double.NegativeInfinity);

    public static CoordinateRange NoLimits => new CoordinateRange(double.NaN, double.NaN);

    public bool IsReal => NumericConversion.IsReal(Max) && NumericConversion.IsReal(Min);

    public bool IsInverted => Min > Max;

    public double TrueMin => Math.Min(Min, Max);

    public double TrueMax => Math.Max(Min, Max);

    public bool Contains(double value)
    {
        return value >= TrueMin && value <= TrueMax;
    }

    public bool Intersects(CoordinateRange other)
    {
        double trueMin = Math.Min(Min, Max);
        double trueMax = Math.Max(Min, Max);
        double otherTrueMin = Math.Min(other.Min, other.Max);
        double otherTrueMax = Math.Max(other.Min, other.Max);

        // other engulfs this
        if (otherTrueMin < trueMin && otherTrueMax > trueMax)
        {
            return true;
        }

        // this engulfs other
        if (trueMin < otherTrueMin && trueMax > otherTrueMax)
        {
            return true;
        }

        // partial intersection
        return Contains(otherTrueMin) || Contains(otherTrueMax);
    }

    /// <summary>
    ///     Return the range of values spanned by the given collection
    /// </summary>
    public static CoordinateRange MinMax(IList<double> values)
    {
        if (values.Any())
        {
            return NotSet;
        }

        double min = double.MaxValue;
        double max = double.MinValue;

        foreach (double value in values)
        {
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        return new CoordinateRange(min, max);
    }

    /// <summary>
    ///     Return the range of values spanned by the given collection (ignoring NaN)
    /// </summary>
    public static CoordinateRange MinMaxNan(IEnumerable<double> values)
    {
        double min = double.NaN;
        double max = double.NaN;

        foreach (double value in values.Where(static value => !double.IsNaN(value)))
        {
            min = double.IsNaN(min) ? value : Math.Min(min, value);
            max = double.IsNaN(max) ? value : Math.Max(max, value);
        }

        return new CoordinateRange(min, max);
    }

    /// <summary>
    ///     Return a new range expanded to include the given point
    /// </summary>
    public CoordinateRange Expanded(double value)
    {
        double min = Math.Min(value, Min);
        double max = Math.Max(value, Max);

        return new CoordinateRange(min, max);
    }

    /// <summary>
    ///     Return a copy of this range where <see cref="Max" /> is never less than <see cref="Min" />
    /// </summary>
    public CoordinateRange Rectified() => Max >= Min ? new CoordinateRange(Min, Max) : new CoordinateRange(Max, Min);
}
