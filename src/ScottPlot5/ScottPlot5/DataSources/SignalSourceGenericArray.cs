﻿namespace ScottPlot.DataSources;

public class SignalSourceGenericArray<T> : SignalSourceBase, ISignalSource
{
    private readonly T[] _ys;

    public override int Length => _ys.Length;

    public SignalSourceGenericArray(T[] ys, double period)
    {
        _ys = ys;
        Period = period;
    }

    public IReadOnlyList<double> GetYs() => NumericConversion.GenericToDoubleArray(_ys);

    public IEnumerable<double> GetYs(int i1, int i2)
    {
        for (int i = i1; i <= i2; i++)
        {
            yield return NumericConversion.GenericToDouble(ref _ys[i]);
        }
    }

    public double GetY(int index) => NumericConversion.GenericToDouble(ref _ys[index]);

    public override SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            double value = NumericConversion.GenericToDouble(ref _ys[i]);
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        return new SignalRangeY(min, max);
    }

    public PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex)
    {
        float xPixel = axes.DataRect.Left + xPixelIndex;
        double xRangeMin = axes.GetCoordinateX(xPixel);
        Debug.Assert(axes.XAxis is not null);
        float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
        double xRangeMax = xRangeMin + xUnitsPerPixel;

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
        float yEnter = axes.GetPixelY((NumericConversion.GenericToDouble(_ys, i1) * YScale) + YOffset);
        float yExit = axes.GetPixelY((NumericConversion.GenericToDouble(_ys, i2) * YScale) + YOffset);

        // determine column span vertically
        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;

        for (int i = i1; i <= i2; i++)
        {
            double value = NumericConversion.GenericToDouble(_ys, i);
            yMin = Math.Min(yMin, value);
            yMax = Math.Max(yMax, value);
        }

        yMin = (yMin * YScale) + YOffset;
        yMax = (yMax * YScale) + YOffset;

        float yBottom = axes.GetPixelY(yMin);
        float yTop = axes.GetPixelY(yMax);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }
}
