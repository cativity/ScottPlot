using System.Numerics;
using ScottPlot.PathStrategies;

namespace ScottPlot.Plottables;

public class VectorField(IVectorFieldSource source) : IPlottable, IHasArrow, IHasLegendText
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    //[Obsolete("use LegendText")]
    //public string Label { get => LegendText; set => LegendText = value; }

    public string LegendText { get; set; } = string.Empty;

    public ArrowStyle ArrowStyle { get; set; } = new ArrowStyle { LineWidth = 2, LineColor = Colors.Black };

    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }

    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }

    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }

    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }

    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }

    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }

    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }

    public float ArrowWidth { get => ArrowStyle.ArrowWidth; set => ArrowStyle.ArrowWidth = value; }

    public float ArrowheadAxisLength { get => ArrowStyle.ArrowheadAxisLength; set => ArrowStyle.ArrowheadAxisLength = value; }

    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }

    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    public IColormap? Colormap { get; set; }

    public IEnumerable<LegendItem> LegendItems => [new LegendItem { LabelText = LegendText, ArrowStyle = ArrowStyle }];

    private IVectorFieldSource Source { get; set; } = source;

    public AxisLimits GetAxisLimits() => Source.GetLimits();

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
        {
            return;
        }

        const float maxLength = 25;

        // TODO: Filter out those that are off-screen? This is subtle, an arrow may be fully off-screen except for its arrowhead, if the blades are long enough.
        RootedPixelVector[] vectors = Source.GetRootedVectors().Select(v => new RootedPixelVector(Axes.GetPixel(v.Point), v.Vector)).ToArray();

        if (vectors.Length == 0)
        {
            return;
        }

        double minMagnitudeSquared = double.PositiveInfinity;
        double maxMagnitudeSquared = double.NegativeInfinity;

        foreach (float magSquared in vectors.Select(v => v.MagnitudeSquared))
        {
            minMagnitudeSquared = Math.Min(minMagnitudeSquared, magSquared);
            maxMagnitudeSquared = Math.Max(maxMagnitudeSquared, magSquared);
        }

        Range range = new Range(Math.Sqrt(minMagnitudeSquared), Math.Sqrt(maxMagnitudeSquared));

        if (range.Min == range.Max)
        {
            range = new Range(0, range.Max);
        }

        for (int i = 0; i < vectors.Length; i++)
        {
            float oldMagnitude = vectors[i].Magnitude;
            double newMagnitude = range.Normalize(oldMagnitude) * maxLength;

            float direction = vectors[i].Angle;
            vectors[i].Vector = new Vector2((float)(Math.Cos(direction) * newMagnitude), (float)(Math.Sin(direction) * newMagnitude));
        }

        double minPixelMag = Math.Sqrt(vectors.Min(x => x.MagnitudeSquared));
        double maxPixelMag = Math.Sqrt(vectors.Max(x => x.MagnitudeSquared));
        Range pixelMagRange = new Range(minPixelMag, maxPixelMag);

        using SKPaint paint = new SKPaint();
        ArrowStyle.LineStyle.ApplyToPaint(paint);
        paint.Style = SKPaintStyle.StrokeAndFill;

        if (Colormap is not null)
        {
            foreach (IGrouping<Color, RootedPixelVector>? group in vectors.ToLookup(v => Colormap.GetColor(v.Magnitude, pixelMagRange)))
            {
                paint.Color = group.Key.ToSKColor();
                RenderVectors(paint, rp.Canvas, group, ArrowStyle);
            }
        }
        else
        {
            RenderVectors(paint, rp.Canvas, vectors, ArrowStyle);
        }
    }

    private static void RenderVectors(SKPaint paint, SKCanvas canvas, IEnumerable<RootedPixelVector> vectors, ArrowStyle arrowStyle)
    {
        using SKPath path = Arrows.GetPath(vectors, arrowStyle);
        canvas.DrawPath(path, paint);
    }
}
