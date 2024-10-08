﻿namespace ScottPlot.Plottables;

/// <summary>
///     This class represents a single radial gauge.
///     It has level and styling options and can be rendered onto an existing bitmap using any radius.
/// </summary>
internal class RadialGauge
{
    /// <summary>
    ///     Location of the base of the gauge (degrees)
    /// </summary>
    public double StartAngle { get; set; }

    /// <summary>
    ///     Current level of this gauge (degrees)
    /// </summary>
    public double SweepAngle { get; set; }

    /// <summary>
    ///     Maximum angular size of the gauge (swept degrees)
    /// </summary>
    public double MaximumSizeAngle { get; set; }

    /// <summary>
    ///     Angle where the background starts (degrees)
    /// </summary>
    public double BackStartAngle { get; set; }

    /// <summary>
    ///     If true angles end clockwise relative to their base
    /// </summary>
    public bool Clockwise { get; set; }

    /// <summary>
    ///     Used internally to get the angle swept by the gauge background. It's equal to 360 degrees if CircularBackground is
    ///     set to true. Also, returns a positive value is the gauge is drawn clockwise and a negative one otherwise
    /// </summary>
    internal double BackAngleSweep
    {
        get
        {
            double maxBackAngle = CircularBackground ? 360 : MaximumSizeAngle;

            if (!Clockwise)
            {
                maxBackAngle = -maxBackAngle;
            }

            return maxBackAngle;
        }
        //private set => BackAngleSweep = value; // Added for the sweepAngle check in DrawArc due to System.Drawing throwing an OutOfMemoryException.
    }

    /// <summary>
    ///     If true the background will always be drawn as a complete circle regardless of MaximumSizeAngle
    /// </summary>
    public bool CircularBackground { get; set; } = true;

    /// <summary>
    ///     Font used to render values at the tip of the gauge
    /// </summary>
    public FontStyle Font { get; set; } = new FontStyle();

    /// <summary>
    ///     Size of the font relative to the line thickness
    /// </summary>
    public double FontSizeFraction { get; set; }

    /// <summary>
    ///     Text to display on top of the label
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    ///     Location of the label text along the length of the gauge.
    ///     Low values place the label near the base and high values place the label at its tip.
    /// </summary>
    public double LabelPositionFraction { get; set; }

    /// <summary>
    ///     Size of the gauge (pixels)
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    ///     Color of the gauge foreground
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Color of the gauge background
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    ///     Style of the base of the gauge
    /// </summary>
    public SKStrokeCap StartCap { get; set; } = SKStrokeCap.Round; // Perhaps we should use our own enumeration and implement our own custom drawing

    /// <summary>
    ///     Style of the tip of the gauge
    /// </summary>
    public SKStrokeCap EndCap { get; set; } = SKStrokeCap.Round; // Perhaps we should use our own enumeration and implement our own custom drawing

    /// <summary>
    ///     Defines the location of each gauge relative to the start angle and distance from the center
    /// </summary>
    public RadialGaugeMode Mode { get; set; }

    /// <summary>
    ///     Indicates whether or not labels will be rendered as text
    /// </summary>
    public bool ShowLabels { get; set; }

    /// <summary>
    ///     Render the gauge onto an existing Bitmap
    /// </summary>
    public virtual void Render(RenderPack rp, float radius)
    {
        RenderBackground(rp, radius);
        RenderGaugeForeground(rp, radius);
        RenderGaugeLabels(rp, radius);
    }

    private void RenderBackground(RenderPack rp, float radius)
    {
        if (Mode == RadialGaugeMode.SingleGauge)
        {
            return;
        }

        using SKPaint skPaint = new SKPaint();

        skPaint.IsAntialias = true;
        skPaint.Style = SKPaintStyle.Stroke;
        skPaint.StrokeWidth = (float)Width;
        skPaint.StrokeCap = StartCap;
        skPaint.Color = new SKColor(BackgroundColor.ARGB);

        using SKPath skPath = new SKPath();

        skPath.AddArc(new SKRect(rp.FigureRect.BottomCenter.X - radius,
                                 rp.FigureRect.LeftCenter.Y - radius,
                                 rp.FigureRect.BottomCenter.X + radius,
                                 rp.FigureRect.LeftCenter.Y + radius),
                      (float)BackStartAngle,
                      (float)BackAngleSweep);

        rp.Canvas.DrawPath(skPath, skPaint);
    }

    public void RenderGaugeForeground(RenderPack rp, float radius)
    {
        using SKPaint skPaint = new SKPaint();

        skPaint.IsAntialias = true;
        skPaint.Style = SKPaintStyle.Stroke;
        skPaint.StrokeWidth = (float)Width;
        skPaint.StrokeCap = StartCap;
        skPaint.Color = new SKColor(Color.ARGB);

        using SKPath skPath = new SKPath();

        skPath.AddArc(new SKRect(rp.FigureRect.BottomCenter.X - radius,
                                 rp.FigureRect.LeftCenter.Y - radius,
                                 rp.FigureRect.BottomCenter.X + radius,
                                 rp.FigureRect.LeftCenter.Y + radius),
                      (float)StartAngle,
                      (float)SweepAngle);

        rp.Canvas.DrawPath(skPath, skPaint);
    }

    private const double _degPerRad = 180.0 / Math.PI;

    private void RenderGaugeLabels(RenderPack rp, float radius)
    {
        if (!ShowLabels || Label.Length == 0)
        {
            return;
        }

        using SKPaint skPaint = new SKPaint();

        skPaint.TextSize = (float)Width * (float)FontSizeFraction;
        skPaint.IsAntialias = true;
        skPaint.SubpixelText = true;
        skPaint.Color = new SKColor(Font.Color.ARGB);
        skPaint.Typeface = Font.Typeface;

        // Text is measured (in linear form) and converted to angular dimensions
        SKRect textBounds = new SKRect();
        skPaint.MeasureText($"{Label}", ref textBounds);
        double textAngle = _degPerRad * textBounds.Width / radius;

        // We compute the angular location where the label has to be drawn.
        double angle = ReduceAngle(StartAngle + (SweepAngle * LabelPositionFraction) - (textAngle / 2));
        bool isBelow = angle is <= 180 and > 0;

        // This is a very dirty trick to avoid label clipping at the gauge's very end.
        float additionalSpace = 0;

        if (LabelPositionFraction is 1 or 0)
        {
            additionalSpace += (SweepAngle > 0) == isBelow ? 1 : -1;
        }

        // We use this trick consisting in the inversion of path so that the orientation of the text is correct (not inverted). Other alternatives would be to flip the canvas, draw the text and restore it.
        if ((SweepAngle > 0) == isBelow) // This is a XNOR operator. It only executes if TRUE-TRUE or FALSE-FALSE
        {
            ReversePath();
        }

        // Create the path where the text will be drawn
        using SKPath skPath = new SKPath();

        skPath.AddArc(new SKRect(rp.FigureRect.BottomCenter.X - radius,
                                 rp.FigureRect.LeftCenter.Y - radius,
                                 rp.FigureRect.BottomCenter.X + radius,
                                 rp.FigureRect.LeftCenter.Y + radius),
                      (float)StartAngle,
                      (float)SweepAngle);

        using SKPathMeasure skMeasure = new SKPathMeasure(skPath);

        SKPoint skPoint = new SKPoint
        {
            Y = -textBounds.MidY, // Displacement along the y axis (radial-wise), so we can center the text on the gauge path
            X = ((float)(LabelPositionFraction / 2) * (skMeasure.Length - textBounds.Width))
                + additionalSpace // Displacement along the x axis (the length of the path), so that we can set the text at any position along the path
        };

        rp.Canvas.DrawTextOnPath(Label, skPath, skPoint, skPaint);

        return;

        // Invert parameters so that the path is reversed
        void ReversePath()
        {
            StartAngle += SweepAngle;
            SweepAngle = -SweepAngle;
            LabelPositionFraction = 1 - LabelPositionFraction;
        }
    }

    /// <summary>
    ///     Reduces an angle into the range [0°-360°].
    ///     Angles greater than 360 will roll-over (370º becomes 10º).
    ///     Angles less than 0 will roll-under (-10º becomes 350º).
    /// </summary>
    /// <param name="angle">Angle value</param>
    /// <returns>Angle whithin [0°-360°]</returns>
    public static double ReduceAngle(double angle)
    {
        angle %= 360;

        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }
}
