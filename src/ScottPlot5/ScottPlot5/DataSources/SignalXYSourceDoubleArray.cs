namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray : ISignalXYSource
{
    private readonly double[] _xs;
    private readonly double[] _ys;

    public int Count => _xs.Length;

    public bool Rotated { get; set; }

    public double XOffset { get; set; }

    public double YOffset { get; set; }

    public double YScale { get; set; } = 1;

    public double XScale { get; set; } = 1;

    public int MinimumIndex { get; set; }

    public int MaximumIndex { get; set; }

    public SignalXYSourceDoubleArray(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");
        }

        _xs = xs;
        _ys = ys;
        MaximumIndex = xs.Length - 1;
    }

    public AxisLimits GetAxisLimits()
    {
        if (_xs.Length == 0)
        {
            return AxisLimits.NoLimits;
        }

        double xMin = (_xs[MinimumIndex] * XScale) + XOffset;
        double xMax = (_xs[MaximumIndex] * XScale) + XOffset;

        CoordinateRange xRange = new CoordinateRange(xMin, xMax);
        CoordinateRange yRange = GetRangeY(MinimumIndex, MaximumIndex);

        return Rotated ? new AxisLimits(yRange, xRange) : new AxisLimits(xRange, yRange);
    }

    public Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
        => Rotated ? GetPixelsToDrawVertically(rp, axes, connectStyle) : GetPixelsToDrawHorizontally(rp, axes, connectStyle);

    private Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] pointBefore, int dataIndexFirst) = GetFirstPointX(axes);
        (Pixel[] pointAfter, int dataIndexLast) = GetLastPointX(axes);
        IndexRange visibleRange = new IndexRange(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> visiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
                                                     .Select(pxColumn => GetColumnPixelsX(pxColumn, visibleRange, rp, axes))
                                                     .SelectMany(x => x);

        Pixel[] leftOutsidePoint = pointBefore,
                rightOutsidePoint = pointAfter;

        if (axes.XAxis?.Range.Span < 0)
        {
            leftOutsidePoint = pointAfter;
            rightOutsidePoint = pointBefore;
        }

        // combine with one extra point before and after
        Pixel[] points = [.. leftOutsidePoint, .. visiblePoints, .. rightOutsidePoint];

        // use interpolation at the edges to prevent points from going way off the screen
        if (leftOutsidePoint.Length > 0)
        {
            SignalInterpolation.InterpolateBeforeX(rp, points, connectStyle);
        }

        if (rightOutsidePoint.Length > 0)
        {
            SignalInterpolation.InterpolateAfterX(rp, points, connectStyle);
        }

        return points;
    }

    private Pixel[] GetPixelsToDrawVertically(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        // determine the range of data in view
        (Pixel[] pointBefore, int dataIndexFirst) = GetFirstPointY(axes);
        (Pixel[] pointAfter, int dataIndexLast) = GetLastPointY(axes);
        IndexRange visibleRange = new IndexRange(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> visiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Height))
                                                     .Select(pxRow => GetColumnPixelsY(pxRow, visibleRange, rp, axes))
                                                     .SelectMany(x => x);

        Pixel[] bottomOutsidePoint = pointBefore;
        Pixel[] topOutsidePoint = pointAfter;

        if (axes.YAxis?.Range.Span < 0)
        {
            bottomOutsidePoint = pointAfter;
            topOutsidePoint = pointBefore;
        }

        // combine with one extra point before and after
        Pixel[] points = [.. bottomOutsidePoint, .. visiblePoints, .. topOutsidePoint];

        // use interpolation at the edges to prevent points from going way off the screen
        if (bottomOutsidePoint.Length > 0)
        {
            SignalInterpolation.InterpolateBeforeY(rp, points, connectStyle);
        }

        if (topOutsidePoint.Length > 0)
        {
            SignalInterpolation.InterpolateAfterY(rp, points, connectStyle);
        }

        return points;
    }

    /// <summary>
    ///     Return the vertical range covered by data between the given indices (inclusive)
    /// </summary>
    private CoordinateRange GetRangeY(int index1, int index2)
    {
        double min = _ys[index1];
        double max = _ys[index1];

        int minIndex = Math.Min(index1, index2);
        int maxIndex = Math.Max(index1, index2);

        for (int i = minIndex; i <= maxIndex; i++)
        {
            min = Math.Min(_ys[i], min);
            max = Math.Max(_ys[i], max);
        }

        return new CoordinateRange((min * YScale) + YOffset, (max * YScale) + YOffset);
    }

    /// <summary>
    ///     Get the index associated with the given X position
    /// </summary>
    private int GetIndex(double x)
    {
        IndexRange range = new IndexRange(MinimumIndex, MaximumIndex);

        return GetIndex(x, range);
    }

    /// <summary>
    ///     Get the index associated with the given X position limited to the given range
    /// </summary>
    private int GetIndex(double x, IndexRange indexRange)
    {
        (_, int index) = SearchIndex(x, indexRange);

        return index;
    }

    /// <summary>
    ///     Given a pixel column, return the pixels to render its line.
    ///     If the column contains no data, no pixels are returned.
    ///     If the column contains one point, return that one pixel.
    ///     If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetColumnPixelsX(int pixelColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float xPixel = pixelColumnIndex + rp.DataRect.Left;
        Debug.Assert(axes.XAxis is not null);
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + (unitsPerPixelX * pixelColumnIndex);
        double end = start + unitsPerPixelX;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelX * .01;
        end += overlap;

        (int startIndex, _) = SearchIndex(start, rng);
        (int endIndex, _) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;

        yield return new Pixel(xPixel, axes.GetPixelY((_ys[firstIndex] * YScale) + YOffset)); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(startIndex, lastIndex); //YOffset is added in GetRangeY

            if (_ys[firstIndex] > _ys[lastIndex])
            {
                //signal amplitude is decreasing, so we'll return the maximum before the minimum
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
            }
            else
            {
                //signal amplitude is increasing, so we'll return the minimum before the maximum
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
                yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
            }
        }

        if (pointsInRange > 1)
        {
            yield return new Pixel(xPixel, axes.GetPixelY((_ys[lastIndex] * YScale) + YOffset)); // exit
        }
    }

    /// <summary>
    ///     Given a pixel column, return the pixels to render its line.
    ///     If the column contains no data, no pixels are returned.
    ///     If the column contains one point, return that one pixel.
    ///     If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetColumnPixelsY(int rowColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        // here rowColumnIndex will count upwards from the bottom, but pixels are measured from the top of the plot
        float yPixel = rp.DataRect.Bottom - rowColumnIndex;
        Debug.Assert(axes.YAxis is not null);
        double unitsPerPixelY = axes.YAxis.Height / rp.DataRect.Height;
        double start = axes.YAxis.Min + (unitsPerPixelY * rowColumnIndex);
        double end = start + unitsPerPixelY;

        // add slight overlap to prevent floating point errors from missing points
        // https://github.com/ScottPlot/ScottPlot/issues/3665
        double overlap = unitsPerPixelY * .01;
        end += overlap;

        (int startIndex, _) = SearchIndex(start, rng);
        (int endIndex, _) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endIndex - startIndex);

        if (pointsInRange == 0)
        {
            yield break;
        }

        int firstIndex = startIndex < endIndex ? startIndex : startIndex - 1;
        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex;

        yield return new Pixel(axes.GetPixelX((_ys[firstIndex] * YScale) + YOffset), yPixel); // enter

        if (pointsInRange > 2)
        {
            CoordinateRange yRange = GetRangeY(firstIndex, lastIndex);

            if (_ys[firstIndex] > _ys[lastIndex])
            {
                //signal amplitude is decreasing, so we'll return the maximum before the minimum
                yield return new Pixel(axes.GetPixelX(yRange.Max), yPixel); // max
                yield return new Pixel(axes.GetPixelX(yRange.Min), yPixel); // min
            }
            else
            {
                //signal amplitude is increasing, so we'll return the minimum before the maximum
                yield return new Pixel(axes.GetPixelX(yRange.Min), yPixel); // min
                yield return new Pixel(axes.GetPixelX(yRange.Max), yPixel); // max
            }
        }

        if (pointsInRange > 1)
        {
            yield return new Pixel(axes.GetPixelX((_ys[lastIndex] * YScale) + YOffset), yPixel); // exit
        }
    }

    /// <summary>
    ///     If data is off to the screen to the left,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointX(IAxes axes)
    {
        if (_xs.Length == 1)
        {
            return ([], MinimumIndex);
        }

        Debug.Assert(axes.XAxis is not null);

        (int firstPointIndex, _) =
            SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Min : axes.XAxis.Max); // if axis is reversed first index will on the right limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeX = axes.GetPixelX((_xs[firstPointIndex - 1] * XScale) + XOffset);
            float beforeY = axes.GetPixelY((_ys[firstPointIndex - 1] * YScale) + YOffset);
            Pixel beforePoint = new Pixel(beforeX, beforeY);

            return ([beforePoint], firstPointIndex);
        }

        return ([], MinimumIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the bottom,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointBefore, int firstIndex) GetFirstPointY(IAxes axes)
    {
        if (_xs.Length == 1)
        {
            return ([], MinimumIndex);
        }

        Debug.Assert(axes.YAxis is not null);

        (int firstPointIndex, _) =
            SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Min : axes.YAxis.Max); // if axis is reversed first index will on the top limit of the plot

        if (firstPointIndex > MinimumIndex)
        {
            float beforeY = axes.GetPixelY((_xs[firstPointIndex - 1] * XScale) + XOffset);
            float beforeX = axes.GetPixelX((_ys[firstPointIndex - 1] * YScale) + YOffset);
            Pixel beforePoint = new Pixel(beforeX, beforeY);

            return ([beforePoint], firstPointIndex);
        }

        return ([], MinimumIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the right,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointAfter, int lastIndex) GetLastPointX(IAxes axes)
    {
        if (_xs.Length == 1)
        {
            return ([], MaximumIndex);
        }

        Debug.Assert(axes.XAxis is not null);

        (int lastPointIndex, _) =
            SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Max : axes.XAxis.Min); // if axis is reversed last index will on the left limit of the plot

        if (lastPointIndex <= MaximumIndex)
        {
            float afterX = axes.GetPixelX((_xs[lastPointIndex] * XScale) + XOffset);
            float afterY = axes.GetPixelY((_ys[lastPointIndex] * YScale) + YOffset);
            Pixel afterPoint = new Pixel(afterX, afterY);

            return ([afterPoint], lastPointIndex - 1);
        }

        return ([], MaximumIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the top,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointAfter, int lastIndex) GetLastPointY(IAxes axes)
    {
        if (_xs.Length == 1)
        {
            return ([], MaximumIndex);
        }

        Debug.Assert(axes.YAxis is not null);

        (int lastPointIndex, _) =
            SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Max : axes.YAxis.Min); // if axis is reversed last index will on the bottom limit of the plot

        if (lastPointIndex > MaximumIndex)
        {
            return ([], MaximumIndex);
        }

        float afterY = axes.GetPixelY((_xs[lastPointIndex] * XScale) + XOffset);
        float afterX = axes.GetPixelX((_ys[lastPointIndex] * YScale) + YOffset);
        Pixel afterPoint = new Pixel(afterX, afterY);

        return ([afterPoint], lastPointIndex - 1);
    }

    /// <summary>
    ///     Search the index associated with the given X position
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x)
    {
        IndexRange range = new IndexRange(MinimumIndex, MaximumIndex);

        return SearchIndex(x, range);
    }

    /// <summary>
    ///     Search the index associated with the given X position limited to the given range
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x, IndexRange indexRange)
    {
        int index = Array.BinarySearch(_xs, indexRange.Min, indexRange.Length, (x - XOffset) / XScale);

        // If x is not exactly matched to any value in Xs, BinarySearch returns a negative number. We can bitwise negation to obtain the position where x would be inserted (i.e., the next highest index).
        // If x is below the min Xs, BinarySearch returns -1. Here, bitwise negation returns 0 (i.e., x would be inserted at the first index of the array).
        // If x is above the max Xs, BinarySearch returns -maxIndex. Bitwise negation of this value returns maxIndex + 1 (i.e., the position after the last index). However, this index is beyond the array bounds, so we return the final index instead.
        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return (SearchedPosition: index, LimitedIndex: index > indexRange.Max ? indexRange.Max : index);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i = 0; i < _xs.Length; i++)
        {
            double dX = Rotated
                            ? ((_ys[i] * YScale) + YOffset - mouseLocation.X) * renderInfo.PxPerUnitX
                            : ((_xs[i] * XScale) + XOffset - mouseLocation.X) * renderInfo.PxPerUnitX;

            double dY = Rotated
                            ? ((_xs[i] * XScale) + XOffset - mouseLocation.Y) * renderInfo.PxPerUnitY
                            : ((_ys[i] * YScale) + YOffset - mouseLocation.Y) * renderInfo.PxPerUnitY;

            double distanceSquared = (dX * dX) + (dY * dY);

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;

                closestX = Rotated ? (_ys[i] * YScale) + YOffset : (_xs[i] * XScale) + XOffset;
                closestY = Rotated ? (_xs[i] * XScale) + XOffset : (_ys[i] * YScale) + YOffset;

                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared ? new DataPoint(closestX, closestY, closestIndex) : DataPoint.None;
    }

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double mousePosition = Rotated ? mouseLocation.Y : mouseLocation.X;
        int i = GetIndex(mousePosition); // TODO: check the index after too?
        double pxPerPositionUnit = Rotated ? renderInfo.PxPerUnitY : renderInfo.PxPerUnitX;

        double distance = ((_xs[i] * XScale) + XOffset - mousePosition) * pxPerPositionUnit;
        double closestX = Rotated ? (_ys[i] * YScale) + YOffset : (_xs[i] * XScale) + XOffset;
        double closestY = Rotated ? (_xs[i] * XScale) + XOffset : (_ys[i] * YScale) + YOffset;

        return Math.Abs(distance) <= maxDistance ? new DataPoint(closestX, closestY, i) : DataPoint.None;
    }
}
