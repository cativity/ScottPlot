﻿using ScottPlot.Collections;

namespace ScottPlot.DataSources;

public class DataLoggerSource(IList<Coordinates> coordinates)
{
    private static readonly CoordinatesXComparer _xComparer = new CoordinatesXComparer();
    private volatile bool _hasNewData;
    private volatile bool _wasRendered;

    public IList<Coordinates> Coordinates { get; } = coordinates;

    public double XOffset { get; set; }

    public double YOffset { get; set; }

    public double YScale { get; set; } = 1;

    private int LastIndex => Coordinates.Count - 1;

    public bool HasNewData
    {
        get => _hasNewData;
        private set => _hasNewData = value;
    }

    public bool WasRendered
    {
        get => _wasRendered;
        set => _wasRendered = value;
    }

    public void Add(Coordinates coordinates)
    {
        // Check that X coordinates are in ascending order
        if (Coordinates.Count > 0 && coordinates.X < Coordinates[LastIndex].X)
        {
            throw new ArgumentException("X coordinates must be in ascending order", nameof(coordinates));
        }

        Coordinates.Add(coordinates);

        HasNewData = true;
    }

    public void OnRendered()
    {
        HasNewData = false;
        WasRendered = true;
    }

    public CoordinateRange GetRangeX()
        => Coordinates.Count == 0 ? CoordinateRange.NoLimits : new CoordinateRange(Coordinates[0].X + XOffset, Coordinates[^1].X + XOffset);

    public CoordinateRange GetRangeY(CoordinateRange newRangeX)
    {
        if (Coordinates.Count == 0)
        {
            return CoordinateRange.NoLimits;
        }

        int startIndex = GetIndex(newRangeX.Min);
        int endIndex = GetIndex(newRangeX.Max);

        return GetRangeY(startIndex, endIndex);
    }

    public Pixel[] GetPixelsToDrawHorizontally(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        if (Coordinates.Count == 0)
        {
            return [];
        }

        // determine the range of data in view
        (Pixel[] pointBefore, int dataIndexFirst) = GetFirstPointX(axes);
        (Pixel[] pointAfter, int dataIndexLast) = GetLastPointX(axes);
        IndexRange visibleRange = new IndexRange(dataIndexFirst, dataIndexLast);

        // get all points in view
        IEnumerable<Pixel> visiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
                                                     .Select(pxColumn => GetColumnPixelsX(pxColumn, visibleRange, rp, axes))
                                                     .SelectMany(x => x);

        Pixel[] leftOutsidePoint = pointBefore;
        Pixel[] rightOutsidePoint = pointAfter;

        if (axes.XAxis?.Range.Span < 0)
        {
            leftOutsidePoint = pointAfter;
            rightOutsidePoint = pointBefore;
        }

        // duplicate the last point to ensure it is always rendered
        // https://github.com/ScottPlot/ScottPlot/issues/3812
        Debug.Assert(axes.XAxis is not null);
        int lastPointIndex = axes.XAxis.IsInverted() ? dataIndexFirst : dataIndexLast;
        Pixel lastPoint = axes.GetPixel(Coordinates[lastPointIndex]);

        // combine with one extra point before and after
        Pixel[] points = [.. leftOutsidePoint, .. visiblePoints, .. rightOutsidePoint, lastPoint];

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

    public Pixel[] GetPixelsToDrawVertically(RenderPack rp, IAxes axes, ConnectStyle connectStyle)
    {
        if (Coordinates.Count == 0)
        {
            return [];
        }

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

        // duplicate the last point to ensure it is always rendered
        // https://github.com/ScottPlot/ScottPlot/issues/3812
        Debug.Assert(axes.YAxis is not null);
        int lastPointIndex = axes.YAxis.IsInverted() ? dataIndexFirst : dataIndexLast;
        Pixel lastPoint = axes.GetPixel(new Coordinates(Coordinates[lastPointIndex].Y, Coordinates[lastPointIndex].X));

        // combine with one extra point before and after
        Pixel[] points = [.. bottomOutsidePoint, .. visiblePoints, .. topOutsidePoint, lastPoint];

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
        double min = Coordinates[index1].Y;
        double max = Coordinates[index1].Y;

        int minIndex = Math.Min(index1, index2);
        int maxIndex = Math.Max(index1, index2);

        for (int i = minIndex; i <= maxIndex; i++)
        {
            min = Math.Min(Coordinates[i].Y, min);
            max = Math.Max(Coordinates[i].Y, max);
        }

        return new CoordinateRange((min * YScale) + YOffset, (max * YScale) + YOffset);
    }

    /// <summary>
    ///     Get the index associated with the given X position
    /// </summary>
    public int GetIndex(double x)
    {
        IndexRange range = new IndexRange(0, LastIndex);

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

        (int startPosition, int startIndex) = SearchIndex(start, rng);
        (int endPosition, int endIndex) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endPosition - startPosition);

        if (pointsInRange == 0)
        {
            yield break;
        }

        yield return new Pixel(xPixel, axes.GetPixelY((Coordinates[startIndex].Y * YScale) + YOffset)); // enter

        if (pointsInRange <= 1)
        {
            yield break;
        }

        int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex + 1;
        CoordinateRange yRange = GetRangeY(startIndex, lastIndex); //YOffset is added in GetRangeY

        yield return new Pixel(xPixel, axes.GetPixelY(yRange.Min)); // min
        yield return new Pixel(xPixel, axes.GetPixelY(yRange.Max)); // max
        yield return new Pixel(xPixel, axes.GetPixelY((Coordinates[lastIndex].Y * YScale) + YOffset)); // exit
    }

    /// <summary>
    ///     Given a pixel column, return the pixels to render its line.
    ///     If the column contains no data, no pixels are returned.
    ///     If the column contains one point, return that one pixel.
    ///     If the column contains multiple points, return 4 pixels: enter, min, max, and exit
    /// </summary>
    private IEnumerable<Pixel> GetColumnPixelsY(int rowColumnIndex, IndexRange rng, RenderPack rp, IAxes axes)
    {
        float yPixel = rp.DataRect.Bottom - rowColumnIndex;
        Debug.Assert(axes.YAxis is not null);
        double unitsPerPixelY = axes.YAxis.Height / rp.DataRect.Height;
        double start = axes.YAxis.Min + (unitsPerPixelY * rowColumnIndex);
        double end = start + unitsPerPixelY;
        (int startPosition, int startIndex) = SearchIndex(start, rng);
        (int endPosition, int endIndex) = SearchIndex(end, rng);
        int pointsInRange = Math.Abs(endPosition - startPosition);

        if (pointsInRange == 0)
        {
            yield break;
        }

        yield return new Pixel(axes.GetPixelX(Coordinates[startIndex].Y + XOffset), yPixel); // enter

        if (pointsInRange > 1)
        {
            int lastIndex = startIndex < endIndex ? endIndex - 1 : endIndex + 1;
            CoordinateRange yRange = GetRangeY(startIndex, lastIndex);

            yield return new Pixel(axes.GetPixelX(yRange.Min), yPixel); // min
            yield return new Pixel(axes.GetPixelX(yRange.Max), yPixel); // max
            yield return new Pixel(axes.GetPixelX(Coordinates[lastIndex].Y + XOffset), yPixel); // exit
        }
    }

    /// <summary>
    ///     If data is off to the screen to the left,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int firstIndex) GetFirstPointX(IAxes axes)
    {
        if (Coordinates.Count == 1)
        {
            return ([], 0);
        }

        Debug.Assert(axes.XAxis is not null);

        (int firstPointPosition, int firstPointIndex) =
            SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Min : axes.XAxis.Max); // if axis is reversed first index will on the right limit of the plot

        if (firstPointPosition <= 0)
        {
            return ([], 0);
        }

        float beforeX = axes.GetPixelX(Coordinates[firstPointIndex - 1].X + XOffset);
        float beforeY = axes.GetPixelY((Coordinates[firstPointIndex - 1].Y * YScale) + YOffset);
        Pixel beforePoint = new Pixel(beforeX, beforeY);

        return ([beforePoint], firstPointIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the bottom,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsBefore, int firstIndex) GetFirstPointY(IAxes axes)
    {
        if (Coordinates.Count == 1)
        {
            return ([], 0);
        }

        Debug.Assert(axes.YAxis is not null);

        (int firstPointPosition, int firstPointIndex) =
            SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Min : axes.YAxis.Max); // if axis is reversed first index will on the top limit of the plot

        if (firstPointPosition <= 0)
        {
            return ([], 0);
        }

        float beforeY = axes.GetPixelY(Coordinates[firstPointIndex - 1].X + XOffset);
        float beforeX = axes.GetPixelX((Coordinates[firstPointIndex - 1].Y * YScale) + YOffset);
        Pixel beforePoint = new Pixel(beforeX, beforeY);

        return ([beforePoint], firstPointIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the right,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsAfter, int lastIndex) GetLastPointX(IAxes axes)
    {
        if (Coordinates.Count == 1)
        {
            return ([], LastIndex);
        }

        Debug.Assert(axes.XAxis is not null);

        (int lastPointPosition, int lastPointIndex) =
            SearchIndex(axes.XAxis.Range.Span > 0 ? axes.XAxis.Max : axes.XAxis.Min); // if axis is reversed last index will on the left limit of the plot

        if (lastPointPosition > LastIndex)
        {
            return ([], LastIndex);
        }

        float afterX = axes.GetPixelX(Coordinates[lastPointIndex].X + XOffset);
        float afterY = axes.GetPixelY((Coordinates[lastPointIndex].Y * YScale) + YOffset);
        Pixel afterPoint = new Pixel(afterX, afterY);

        return ([afterPoint], lastPointIndex);
    }

    /// <summary>
    ///     If data is off to the screen to the top,
    ///     returns information about the closest point off the screen
    /// </summary>
    private (Pixel[] pointsAfter, int lastIndex) GetLastPointY(IAxes axes)
    {
        if (Coordinates.Count == 1)
        {
            return ([], LastIndex);
        }

        Debug.Assert(axes.YAxis is not null);

        (int lastPointPosition, int lastPointIndex) =
            SearchIndex(axes.YAxis.Range.Span > 0 ? axes.YAxis.Max : axes.YAxis.Min); // if axis is reversed last index will on the bottom limit of the plot

        if (lastPointPosition > LastIndex)
        {
            return ([], LastIndex);
        }

        float afterY = axes.GetPixelY(Coordinates[lastPointIndex].X + XOffset);
        float afterX = axes.GetPixelX((Coordinates[lastPointIndex].Y * YScale) + YOffset);
        Pixel afterPoint = new Pixel(afterX, afterY);

        return ([afterPoint], lastPointIndex);
    }

    /// <summary>
    ///     Search the index associated with the given X position
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x)
    {
        IndexRange range = new IndexRange(0, LastIndex);

        return SearchIndex(x, range);
    }

    /// <summary>
    ///     Search the index associated with the given X position limited to the given range
    /// </summary>
    private (int SearchedPosition, int LimitedIndex) SearchIndex(double x, IndexRange indexRange)
    {
        int index = Coordinates.BinarySearch(indexRange.Min, indexRange.Length, new Coordinates(x - XOffset, 0), _xComparer);

        // If x is not exactly matched to any value in Xs, BinarySearch returns a negative number. We can bitwise negate to obtain the position where x would be inserted (i.e., the next highest index).
        // If x is below the min Xs, BinarySearch returns -1. Here, bitwise negation returns 0 (i.e., x would be inserted at the first index of the array).
        // If x is above the max Xs, BinarySearch returns -maxIndex. Bitwise negation of this value returns maxIndex + 1 (i.e., the position after the last index). However, this index is beyond the array bounds, so we return the final index instead.
        if (index < 0)
        {
            index = ~index; // read BinarySearch() docs
        }

        return (SearchedPosition: index, LimitedIndex: index > indexRange.Max ? indexRange.Max : index);
    }
}

internal class CoordinatesXComparer : IComparer<Coordinates>
{
    public int Compare(Coordinates a, Coordinates b) => a.X.CompareTo(b.X);
}

internal static class ListBinarySearchExtensions
{
    public static int BinarySearch<T>(this IList<T> list, int index, int count, T value, IComparer<T> comparer)
    {
        return list switch
        {
            List<T> listT => listT.BinarySearch(index, count, value, comparer),
            T[] arrayT => Array.BinarySearch(arrayT, index, count, value, comparer),
            CircularBuffer<T> circularBufferT => circularBufferT.BinarySearch(index, count, value, comparer),
            _ => throw new NotSupportedException($"unsupported {nameof(IList<T>)}: {list.GetType().Name}")
        };
    }
}
