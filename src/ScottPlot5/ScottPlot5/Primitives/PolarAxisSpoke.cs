namespace ScottPlot;

/// <summary>
///     A straight line extending outward from the origin
/// </summary>
public class PolarAxisSpoke(Angle angle, double length) : IHasLine
{
    public Angle Angle { get; set; } = angle;

    public double Length { get; set; } = length;

    public LabelStyle LabelStyle { get; } = new LabelStyle();

    public LineStyle LineStyle { get; set; } = new LineStyle { Width = 1, Color = Colors.Black.WithAlpha(.5), };

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public LinePattern LinePattern
    {
        get => LineStyle.Pattern;
        set => LineStyle.Pattern = value;
    }

    public Color LineColor
    {
        get => LineStyle.Color;
        set => LineStyle.Color = value;
    }

    public void Render(RenderPack rp, IAxes axes, SKPaint paint, double labelDistance)
    {
        PolarCoordinates tipPoint = new PolarCoordinates(Length, Angle);
        Pixel tipPixel = axes.GetPixel(tipPoint.CartesianCoordinates) - axes.GetPixel(Coordinates.Origin);
        Drawing.DrawLine(rp.Canvas, paint, new Pixel(0, 0), tipPixel, LineStyle);

        LabelStyle.Text = $"{Angle.Degrees}"; // TODO: use a customizable label formatter

        PolarCoordinates labelPoint = new PolarCoordinates(tipPoint.Radius * labelDistance, tipPoint.Angle);
        Pixel labelPixel = axes.GetPixel(labelPoint.CartesianCoordinates) - axes.GetPixel(Coordinates.Origin);
        PixelRect labelRect = LabelStyle.Measure().Rect(Alignment.MiddleCenter);
        Pixel labelOffset = labelRect.Center - labelRect.TopLeft;
        labelPixel -= labelOffset;
        LabelStyle.Render(rp.Canvas, labelPixel, paint);
    }
}
