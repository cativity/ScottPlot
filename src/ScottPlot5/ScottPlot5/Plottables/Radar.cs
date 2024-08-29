using ScottPlot.StarAxes;

namespace ScottPlot.Plottables;

public class Radar(IReadOnlyList<RadarSeries> series) : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => Series.Select(static s => new LegendItem { LabelText = s.LegendText, FillStyle = s.Fill });

    public LineStyle LineStyle { get; set; } = new LineStyle { Width = 0 };

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }

    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public IStarAxis StarAxis { get; set; } = new PolygonalStarAxis();

    public IReadOnlyList<RadarSeries> Series { get; set; } = series;

    public double Padding { get; set; } = 0.2;

    public double LabelDistance { get; set; } = 1.2;

    public IReadOnlyList<LabelStyle>? Labels { get; set; }

    public AxisLimits GetAxisLimits()
    {
        double radius = 1 + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);
    }

    public virtual void Render(RenderPack rp)
    {
        if (!Series.Any())
        {
            return;
        }

        const float startAngle = -90;
        int seriesArity = Series.Min(static s => s.Values.Count);
        double rotationPerSlice = Math.PI * 2 / seriesArity;

        StarAxis.Render(rp, Axes, 1, seriesArity, (float)(startAngle - (rotationPerSlice * 180 / Math.PI / 2)));

        double maxValue = Series.SelectMany(static s => s.Values).Max();

        if (maxValue == 0)
        {
            return;
        }

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        using SKPaint paint = new SKPaint();
        using SKPath path = new SKPath();
        using SKAutoCanvasRestore _ = new SKAutoCanvasRestore(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (RadarSeries? serie in Series)
        {
            for (int i = 0; i < seriesArity; i++)
            {
                double coordinateRadius = serie.Values[i] / maxValue;
                double theta = GetAngleRadians(rotationPerSlice, i, startAngle);
                Pixel px = PixelFromPolar(coordinateRadius, theta, origin);

                if (i == 0)
                {
                    path.MoveTo(px.ToSKPoint());
                }
                else
                {
                    path.LineTo(px.ToSKPoint());
                }
            }

            path.Close();

            serie.Fill.ApplyToPaint(paint, rp.FigureRect);
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }

        if (Labels is not null)
        {
            for (int i = 0; i < seriesArity; i++)
            {
                if (i >= Labels.Count)
                {
                    break;
                }

                double theta = GetAngleRadians(rotationPerSlice, i, startAngle);
                Pixel px = PixelFromPolar(LabelDistance, theta, origin);

                Labels[i].Render(rp.Canvas, px, paint);
            }
        }
    }

    private static double GetAngleRadians(double rotationPerSliceDegrees, int i, double startAngleDegrees)
        => (rotationPerSliceDegrees * i) + (startAngleDegrees * Math.PI / 180);

    private Pixel PixelFromPolar(double coordinateRadius, double theta, Pixel origin)
    {
        float minX = Math.Abs(Axes.GetPixelX(coordinateRadius) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(coordinateRadius) - origin.Y);
        float radius = Math.Min(minX, minY);

        float x = (float)(radius * Math.Cos(theta));
        float y = (float)(radius * Math.Sin(theta));

        return new Pixel(x, y);
    }
}
