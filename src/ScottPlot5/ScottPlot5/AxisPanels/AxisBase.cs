﻿using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public abstract class AxisBase : LabelStyleProperties
{
    public bool IsVisible { get; set; } = true;

    public abstract Edge Edge { get; }

    public virtual CoordinateRangeMutable Range { get; private set; } = CoordinateRangeMutable.NotSet;

    public float MinimumSize { get; set; }

    public float MaximumSize { get; set; } = float.MaxValue;

    public float SizeWhenNoData { get; set; } = 15;

    public PixelPadding EmptyLabelPadding { get; set; } = new PixelPadding(10, 5);

    public PixelPadding PaddingBetweenTickAndAxisLabels { get; set; } = new PixelPadding(5, 3);

    public PixelPadding PaddingOutsideAxisLabels { get; set; } = new PixelPadding(2, 2);

    public double Min
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Max
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public override string ToString() => base.ToString() + " " + Range;

    public virtual ITickGenerator? TickGenerator { get; set; }

    //[Obsolete("use LabelText, LabelFontColor, LabelFontSize, LabelFontName, etc. or properties of LabelStyle", false)]
    //public LabelStyle Label => LabelStyle;

    public override LabelStyle LabelStyle { get; set; } = new LabelStyle { Text = string.Empty, FontSize = 16, Bold = true, Rotation = -90, };

    public bool ShowDebugInformation { get; set; }

    public LineStyle FrameLineStyle { get; } = new LineStyle { Width = 1, Color = Colors.Black, AntiAlias = false, };

    public TickMarkStyle MajorTickStyle { get; set; } = new TickMarkStyle { Length = 4, Width = 1, Color = Colors.Black, AntiAlias = false, };

    public TickMarkStyle MinorTickStyle { get; set; } = new TickMarkStyle { Length = 2, Width = 1, Color = Colors.Black, AntiAlias = false, };

    public LabelStyle TickLabelStyle { get; set; } = new LabelStyle { Alignment = Alignment.MiddleCenter };

    /// <summary>
    ///     Apply a single color to all axis components: label, tick labels, tick marks, and frame
    /// </summary>
    public void Color(Color color)
    {
        LabelStyle.ForeColor = color;
        TickLabelStyle.ForeColor = color;
        MajorTickStyle.Color = color;
        MinorTickStyle.Color = color;
        FrameLineStyle.Color = color;
    }

    /// <summary>
    ///     Draw a line along the edge of an axis on the side of the data area
    /// </summary>
    public static void DrawFrame(RenderPack rp, PixelRect panelRect, Edge edge, LineStyle lineStyle)
    {
        PixelLine pxLine = edge switch
        {
            Edge.Left => new PixelLine(panelRect.Right, panelRect.Bottom, panelRect.Right, panelRect.Top),
            Edge.Right => new PixelLine(panelRect.Left, panelRect.Bottom, panelRect.Left, panelRect.Top),
            Edge.Bottom => new PixelLine(panelRect.Left, panelRect.Top, panelRect.Right, panelRect.Top),
            Edge.Top => new PixelLine(panelRect.Left, panelRect.Bottom, panelRect.Right, panelRect.Bottom),
            _ => throw new NotImplementedException(edge.ToString()),
        };

        if (edge == Edge.Top && !lineStyle.AntiAlias)
        {
            // move the top frame line slightly down so the vertical pixel snaps
            // to the same level as the top of the left and right frame lines
            // https://github.com/ScottPlot/ScottPlot/pull/3976
            pxLine = pxLine.WithDelta(0, .1f);
        }

        using SKPaint paint = new SKPaint();
        Drawing.DrawLine(rp.Canvas, paint, pxLine, lineStyle);
    }

    private static void DrawTicksHorizontalAxis(RenderPack rp,
                                                LabelStyle label,
                                                PixelRect panelRect,
                                                IEnumerable<Tick> ticks,
                                                IAxis axis,
                                                TickMarkStyle majorStyle,
                                                TickMarkStyle minorStyle)
    {
        if (axis.Edge is not Edge.Bottom and not Edge.Top)
        {
            throw new InvalidOperationException();
        }

        using SKPaint paint = new SKPaint();

        foreach (Tick tick in ticks)
        {
            // draw tick
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            paint.IsAntialias = tick.IsMajor ? majorStyle.AntiAlias : minorStyle.AntiAlias;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;
            float xPx = axis.GetPixel(tick.Position, panelRect);
            float y = axis.Edge == Edge.Bottom ? panelRect.Top : panelRect.Bottom;
            float yEdge = axis.Edge == Edge.Bottom ? y + tickLength : y - tickLength;
            PixelLine pxLine = new PixelLine(xPx, y, xPx, yEdge);
            TickMarkStyle lineStyle = tick.IsMajor ? majorStyle : minorStyle;
            lineStyle.Render(rp.Canvas, paint, pxLine);

            // draw label
            if (string.IsNullOrWhiteSpace(tick.Label) || !label.IsVisible)
            {
                continue;
            }

            label.Text = tick.Label;
            const float pxDistanceFromTick = 2;
            float pxDistanceFromEdge = tickLength + pxDistanceFromTick;
            float yPx = axis.Edge == Edge.Bottom ? y + pxDistanceFromEdge : y - pxDistanceFromEdge;
            Pixel labelPixel = new Pixel(xPx, yPx);

            if (label.Rotation == 0)
            {
                label.Alignment = axis.Edge == Edge.Bottom ? Alignment.UpperCenter : Alignment.LowerCenter;
            }

            label.Render(rp.Canvas, labelPixel, paint);
        }
    }

    private static void DrawTicksVerticalAxis(RenderPack rp,
                                              LabelStyle label,
                                              PixelRect panelRect,
                                              IEnumerable<Tick> ticks,
                                              IAxis axis,
                                              TickMarkStyle majorStyle,
                                              TickMarkStyle minorStyle)
    {
        if (axis.Edge is not Edge.Left and not Edge.Right)
        {
            throw new InvalidOperationException();
        }

        using SKPaint paint = new SKPaint();

        foreach (Tick tick in ticks)
        {
            // draw tick
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            paint.IsAntialias = tick.IsMajor ? majorStyle.AntiAlias : minorStyle.AntiAlias;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;
            float yPx = axis.GetPixel(tick.Position, panelRect);
            float x = axis.Edge == Edge.Left ? panelRect.Right : panelRect.Left;
            float xEdge = axis.Edge == Edge.Left ? x - tickLength : x + tickLength;
            PixelLine pxLine = new PixelLine(x, yPx, xEdge, yPx);
            TickMarkStyle lineStyle = tick.IsMajor ? majorStyle : minorStyle;
            lineStyle.Render(rp.Canvas, paint, pxLine);

            // draw label
            if (string.IsNullOrWhiteSpace(tick.Label) || !label.IsVisible)
            {
                continue;
            }

            label.Text = tick.Label;
            const float pxDistanceFromTick = 5;
            float pxDistanceFromEdge = tickLength + pxDistanceFromTick;
            float xPx = axis.Edge == Edge.Left ? x - pxDistanceFromEdge : x + pxDistanceFromEdge;
            Pixel px = new Pixel(xPx, yPx);

            if (label.Rotation == 0)
            {
                label.Alignment = axis.Edge == Edge.Left ? Alignment.MiddleRight : Alignment.MiddleLeft;
            }

            label.Render(rp.Canvas, px, paint);
        }
    }

    public static void DrawTicks(RenderPack rp,
                                 LabelStyle label,
                                 PixelRect panelRect,
                                 IEnumerable<Tick> ticks,
                                 IAxis axis,
                                 TickMarkStyle majorStyle,
                                 TickMarkStyle minorStyle)
    {
        if (axis.Edge.IsVertical())
        {
            DrawTicksVerticalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
        }
        else
        {
            DrawTicksHorizontalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
        }
    }

    /// <summary>
    ///     Replace the <see cref="TickGenerator" /> with a <see cref="NumericManual" /> pre-loaded with the given ticks.
    /// </summary>
    public void SetTicks(double[] xs, string[] labels)
    {
        if (xs.Length != labels.Length)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(labels)} must have equal length");
        }

        NumericManual manualTickGen = new NumericManual();

        for (int i = 0; i < xs.Length; i++)
        {
            manualTickGen.AddMajor(xs[i], labels[i]);
        }

        TickGenerator = manualTickGen;
    }
}
