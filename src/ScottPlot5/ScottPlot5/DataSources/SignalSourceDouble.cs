namespace ScottPlot.DataSources;

public class SignalSourceDouble : SignalSourceBase, ISignalSource
{
    private readonly IReadOnlyList<double> _ys;

    public override int Length => _ys.Count;

    public SignalSourceDouble(IReadOnlyList<double> ys, double period)
    {
        _ys = ys;
        Period = period;
    }

    public IReadOnlyList<double> GetYs() => _ys;

    public IEnumerable<double> GetYs(int i1, int i2)
    {
        for (int i = i1; i <= i2; i++)
        {
            yield return _ys[i];
        }
    }

    public double GetY(int index) => _ys[index];

    public override SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            min = Math.Min(min, _ys[i]);
            max = Math.Max(max, _ys[i]);
        }

        return new SignalRangeY(min, max);
    }

    public PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex)
    {
        float xPixel = axes.DataRect.Left + xPixelIndex;
        double xRangeMin = axes.GetCoordinateX(xPixel);
        Debug.Assert(axes.XAxis is not null);
        float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
        double xRangeMax = xRangeMin + Math.Abs(xUnitsPerPixel);

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        xRangeMax += xUnitsPerPixel * .01;

        if (!RangeContainsSignal(xRangeMin, xRangeMax))
        {
            return PixelColumn.WithoutData(xPixel);
        }

        // determine column limits horizontally
        int i1 = GetIndex(xRangeMin, true);
        int i2 = GetIndex(xRangeMax, true);

        // first and last Y vales for this column
        float yEnter = axes.GetPixelY((_ys[i1] * YScale) + YOffset);
        float yExit = axes.GetPixelY((_ys[i2] * YScale) + YOffset);

        // column min and max
        SignalRangeY rangeY = GetLimitsY(i1, i2);
        float yBottom = axes.GetPixelY((rangeY.Min * YScale) + YOffset);
        float yTop = axes.GetPixelY((rangeY.Max * YScale) + YOffset);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }
}
