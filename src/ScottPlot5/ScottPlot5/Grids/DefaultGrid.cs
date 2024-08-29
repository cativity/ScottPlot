namespace ScottPlot.Grids;

public class DefaultGrid(IXAxis xAxis, IYAxis yAxis) : IGrid
{
    public bool IsVisible { get; set; } = true;

    public bool IsBeneathPlottables { get; set; } = true;

    public IXAxis XAxis { get; set; } = xAxis;

    public IYAxis YAxis { get; set; } = yAxis;

    public GridStyle XAxisStyle { get; set; } = new GridStyle();

    public GridStyle YAxisStyle { get; set; } = new GridStyle();

    public Color MajorLineColor
    {
        get => XAxisStyle.MajorLineStyle.Color;
        set
        {
            XAxisStyle.MajorLineStyle.Color = value;
            YAxisStyle.MajorLineStyle.Color = value;
        }
    }

    public Color MinorLineColor
    {
        set
        {
            XAxisStyle.MinorLineStyle.Color = value;
            YAxisStyle.MinorLineStyle.Color = value;
        }
    }

    public float MajorLineWidth
    {
        set
        {
            XAxisStyle.MajorLineStyle.Width = value;
            YAxisStyle.MajorLineStyle.Width = value;
        }
    }

    public float MinorLineWidth
    {
        set
        {
            XAxisStyle.MinorLineStyle.Width = value;
            YAxisStyle.MinorLineStyle.Width = value;
        }
    }

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
        {
            return;
        }

        if (XAxisStyle.IsVisible)
        {
            double minX = Math.Min(XAxis.Min, XAxis.Max);
            double maxX = Math.Max(XAxis.Min, XAxis.Max);
            List<Tick> xTicks = XAxis.TickGenerator?.Ticks.Where(x => x.Position >= minX && x.Position <= maxX).ToList() ?? [];
            XAxisStyle.Render(rp, XAxis, xTicks);
        }

        if (YAxisStyle.IsVisible)
        {
            double minY = Math.Min(YAxis.Min, YAxis.Max);
            double maxY = Math.Max(YAxis.Min, YAxis.Max);
            List<Tick> yTicks = YAxis.TickGenerator?.Ticks.Where(x => x.Position >= minY && x.Position <= maxY).ToList() ?? [];
            YAxisStyle.Render(rp, YAxis, yTicks);
        }
    }
}
