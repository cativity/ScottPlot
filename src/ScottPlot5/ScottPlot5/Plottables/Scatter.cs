﻿using ScottPlot.PathStrategies;

namespace ScottPlot.Plottables;

public class Scatter(IScatterSource data) : IPlottable, IHasLine, IHasMarker, IHasLegendText
{
    //[Obsolete("use LegendText")]
    //public string Label { get => LegendText; set => LegendText = value; }

    public string LegendText { get; set; } = string.Empty;

    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; set; } = new LineStyle { Width = 1 };

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }

    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public int MinRenderIndex { get => Data.MinRenderIndex; set => Data.MinRenderIndex = value; }

    public int MaxRenderIndex { get => Data.MaxRenderIndex; set => Data.MaxRenderIndex = value; }

    public MarkerStyle MarkerStyle { get; set; } = new MarkerStyle { LineWidth = 1, Size = 5, Shape = MarkerShape.FilledCircle, };

    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }

    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }

    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }

    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }

    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }

    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public IScatterSource Data { get; } = data;

    public bool FillY { get; set; }

    public bool FillYBelow { get; set; } = true;

    public bool FillYAbove { get; set; } = true;

    public double FillYValue { get; set; }

    public Color FillYAboveColor { get; set; } = Colors.Blue.WithAlpha(.2);

    public Color FillYBelowColor { get; set; } = Colors.Blue.WithAlpha(.2);

    public Color FillYColor
    {
        get => FillYAboveColor;
        set
        {
            FillYAboveColor = value;
            FillYBelowColor = value;
        }
    }

    public List<ColorPosition> ColorPositions { get; set; } = [];

    public record struct ColorPosition(Color Color, double Position);

    public double OffsetX { get; set; }

    public double OffsetY { get; set; }

    public double ScaleX { get; set; } = 1;

    public double ScaleY { get; set; } = 1;

    /// <summary>
    ///     The style of lines to use when connecting points.
    /// </summary>
    public ConnectStyle ConnectStyle = ConnectStyle.Straight;

    /// <summary>
    ///     Controls whether points are connected by smooth or straight lines
    /// </summary>
    public bool Smooth
    {
        set => PathStrategy = value ? new CubicSpline() : new Straight();
    }

    /// <summary>
    ///     Setting this value enables <see cref="Smooth" /> and sets the curve tension.
    ///     Low tensions tend to "overshoot" data points.
    ///     High tensions begin to approach connecting points with straight lines.
    /// </summary>
    public double SmoothTension
    {
        get => PathStrategy is CubicSpline cs ? cs.Tension : 0;
        set => PathStrategy = new CubicSpline { Tension = value };
    }

    /// <summary>
    ///     Strategy to use for generating the path used to connect points
    /// </summary>
    public IPathStrategy PathStrategy { get; set; } = new Straight();

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
            MarkerStyle.LineColor = value;
        }
    }

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new ExpandingAxisLimits(Data.GetLimits());

        if (FillY)
        {
            limits.ExpandY(FillYValue);
        }

        return new AxisLimits((limits.Left * ScaleX) + OffsetX,
                              (limits.Right * ScaleX) + OffsetX,
                              (limits.Bottom * ScaleY) + OffsetY,
                              (limits.Top * ScaleY) + OffsetY);
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle, LineStyle);

    private Gradient CreateXAxisGradient(RenderPack rp)
    {
        Debug.Assert(Axes.XAxis is not null);
        float xMin = (float)Axes.XAxis.GetCoordinate(rp.DataRect.Left, rp.DataRect);
        float xMax = (float)Axes.XAxis.GetCoordinate(rp.DataRect.Right, rp.DataRect);

        List<double> colorPositions = ColorPositions
                                      .Select(static x => x.Position)
                                      .Concat([xMin, xMax])
                                      .Where(i => i >= xMin && i <= xMax)
                                      .OrderBy(static i => i).ToList();

        IEnumerable<Color> colors = colorPositions.Select(Interpolate);
        float[] colorPositionsFrac = colorPositions.Select(GetAxisFractionX).ToArray();

        return new Gradient
        {
            GradientType = GradientType.Linear,
            AlignmentStart = Alignment.MiddleLeft,
            AlignmentEnd = Alignment.MiddleRight,
            ColorPositions = colorPositionsFrac,
            Colors = colors.ToArray(),
        };

        // TODO: move this logic to a positioned color colormap class
        Color Interpolate(double val)
        {
            if (ColorPositions.Select(x => x.Position).All(i => val < i))
            {
                return ColorPositions[0].Color;
            }

            if (ColorPositions.Select(x => x.Position).All(i => val > i))
            {
                return ColorPositions[^1].Color;
            }

            int lIdx = -1;
            int rIdx = -1;

            for (int i = 0; i < ColorPositions.Count; i++)
            {
                if (ColorPositions[i].Position <= val && (lIdx < 0 || ColorPositions[i].Position > ColorPositions[lIdx].Position))
                {
                    lIdx = i;
                }

                if (ColorPositions[i].Position >= val && (rIdx < 0 || ColorPositions[i].Position < ColorPositions[rIdx].Position))
                {
                    rIdx = i;
                }
            }

            if (lIdx == rIdx)
            {
                return ColorPositions[lIdx].Color;
            }

            double factor = (val - ColorPositions[lIdx].Position) / (ColorPositions[rIdx].Position - ColorPositions[lIdx].Position);

            return ColorPositions[lIdx].Color.InterpolateRgb(ColorPositions[rIdx].Color, factor);
        }

        float GetAxisFractionX(double x)
        {
            double distanceFromLeft = Axes.GetPixelX(x) - Axes.DataRect.Left;
            double width = Axes.DataRect.Right - Axes.DataRect.Left;

            return (float)(distanceFromLeft / width);
        }
    }

    public virtual void Render(RenderPack rp)
    {
        // TODO: can this be done with an iterator to avoid copying?
        IReadOnlyList<Coordinates> coordinates = Data.GetScatterPoints();

        Pixel[] markerPixels = new Pixel[coordinates.Count];

        for (int i = 0; i < coordinates.Count; i++)
        {
            double x = (coordinates[i].X * ScaleX) + OffsetX;
            double y = (coordinates[i].Y * ScaleY) + OffsetY;
            markerPixels[i] = Axes.GetPixel(new Coordinates(x, y));
        }

        if (markerPixels.Length == 0)
        {
            return;
        }

        Pixel[] linePixels = ConnectStyle switch
        {
            ConnectStyle.Straight => markerPixels,
            ConnectStyle.StepHorizontal => GetStepDisplayPixels(markerPixels, true),
            ConnectStyle.StepVertical => GetStepDisplayPixels(markerPixels, false),
            _ => throw new NotImplementedException($"unsupported {nameof(ConnectStyle)}: {ConnectStyle}"),
        };

        using SKPaint paint = new SKPaint();
        using SKPath path = PathStrategy.GetPath(linePixels);

        if (FillY)
        {
            FillStyle fs = new FillStyle { IsVisible = true, };

            if (ColorPositions.Count > 0)
            {
                fs.Hatch = CreateXAxisGradient(rp);
            }

            PixelRect dataPxRect = new PixelRect(markerPixels);

            PixelRect rect = new PixelRect(linePixels);
            Debug.Assert(Axes.YAxis is not null);
            float yValuePixel = Axes.YAxis.GetPixel(FillYValue, rp.DataRect);

            using SKPath fillPath = new SKPath(path);
            fillPath.LineTo(rect.Right, yValuePixel);
            fillPath.LineTo(rect.Left, yValuePixel);

            bool midWay = yValuePixel < dataPxRect.Bottom && yValuePixel > dataPxRect.Top;
            bool belowOnly = yValuePixel <= dataPxRect.Top;
            bool aboveOnly = yValuePixel >= dataPxRect.Bottom;

            if (midWay || aboveOnly)
            {
                PixelRect rectAbove = new PixelRect(rp.DataRect.Left, rp.DataRect.Right, yValuePixel, rect.Top);
                rp.CanvasState.Save();
                rp.CanvasState.Clip(rectAbove);
                fs.Color = ColorPositions.Count > 0 ? Colors.Black : FillYAboveColor;
                Drawing.DrawPath(rp.Canvas, paint, fillPath, fs, rectAbove);
                rp.CanvasState.Restore();
            }

            if (midWay || belowOnly)
            {
                PixelRect rectBelow = new PixelRect(rp.DataRect.Left, rp.DataRect.Right, rect.Bottom, yValuePixel);
                rp.CanvasState.Save();
                rp.CanvasState.Clip(rectBelow);
                fs.Color = ColorPositions.Count > 0 ? Colors.Black : FillYBelowColor;
                Drawing.DrawPath(rp.Canvas, paint, fillPath, fs, rectBelow);
                rp.CanvasState.Restore();
            }
        }

        Drawing.DrawLines(rp.Canvas, paint, path, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }

    /// <summary>
    ///     Convert scatter plot points (connected by diagonal lines) to step plot points (connected by right angles)
    ///     by inserting an extra point between each of the original data points to result in L-shaped steps.
    /// </summary>
    /// <param name="pixels">Array of corner positions</param>
    /// <param name="right">Indicates that a line will extend to the right before rising or falling.</param>
    public static Pixel[] GetStepDisplayPixels(Pixel[] pixels, bool right)
    {
        Pixel[] pixelsStep = new Pixel[(pixels.Length * 2) - 1];

        int offsetX = right ? 1 : 0;
        int offsetY = right ? 0 : 1;

        for (int i = 0; i < pixels.Length - 1; i++)
        {
            pixelsStep[i * 2] = pixels[i];
            pixelsStep[(i * 2) + 1] = new Pixel(pixels[i + offsetX].X, pixels[i + offsetY].Y);
        }

        pixelsStep[^1] = pixels[^1];

        return pixelsStep;
    }
}
