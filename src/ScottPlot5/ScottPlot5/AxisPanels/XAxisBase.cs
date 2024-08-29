namespace ScottPlot.AxisPanels;

public abstract class XAxisBase : AxisBase, IXAxis
{
    public double Width => Range.Span;

    protected XAxisBase() => LabelRotation = 0;

    //[Obsolete("use LabelText, LabelFontColor, LabelFontSize, LabelFontName, etc. or properties of LabelStyle", false)]
    public LabelStyle Label => LabelStyle;

    public virtual float Measure()
    {
        if (!IsVisible)
        {
            return 0;
        }

        if (!Range.HasBeenSet)
        {
            return SizeWhenNoData;
        }

        using SKPaint paint = new SKPaint();

        float tickHeight = MajorTickStyle.Length;

        float maxTickLabelHeight = TickGenerator?.Ticks.Length > 0 ? TickGenerator.Ticks.Max(x => TickLabelStyle.Measure(x.Label, paint).Height) : 0;

        float axisLabelHeight = string.IsNullOrEmpty(LabelStyle.Text)
                                    ? EmptyLabelPadding.Vertical
                                    : LabelStyle.Measure(LabelText, paint).LineHeight
                                      + PaddingBetweenTickAndAxisLabels.Vertical
                                      + PaddingOutsideAxisLabels.Vertical;

        return tickHeight + maxTickLabelHeight + axisLabelHeight;
    }

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        double unitsFromLeftEdge = position - Min;
        float pxFromEdge = (float)(unitsFromLeftEdge * pxPerUnit);

        return dataArea.Left + pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Width / Width;
        float pxFromLeftEdge = pixel - dataArea.Left;
        double unitsFromEdge = pxFromLeftEdge / pxPerUnit;

        return Min + unitsFromEdge;
    }

    private static PixelRect GetPanelRectangleBottom(PixelRect dataRect, float size, float offset)
        => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Bottom + offset + size, dataRect.Bottom + offset);

    private static PixelRect GetPanelRectangleTop(PixelRect dataRect, float size, float offset)
        => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Top - offset, dataRect.Top - offset - size);

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
        => Edge == Edge.Bottom ? GetPanelRectangleBottom(dataRect, size, offset) : GetPanelRectangleTop(dataRect, size, offset);

    public virtual void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
        {
            return;
        }

        using SKPaint paint = new SKPaint();

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        float y = Edge == Edge.Bottom ? panelRect.Bottom - PaddingOutsideAxisLabels.Vertical : panelRect.Top + PaddingOutsideAxisLabels.Vertical;

        Pixel labelPoint = new Pixel(panelRect.HorizontalCenter, y);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, LabelFontColor);
        }

        LabelAlignment = Alignment.LowerCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, paint);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator?.Ticks ?? [], this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea) => distance * dataArea.Width / Width;

    public double GetCoordinateDistance(float distance, PixelRect dataArea) => distance / (dataArea.Width / Width);

    public void RegenerateTicks(PixelLength size)
    {
        using SKPaint paint = new SKPaint();
        TickLabelStyle.ApplyToPaint(paint);
        TickGenerator?.Regenerate(Range.ToCoordinateRange, Edge, size, paint, TickLabelStyle);
    }
}
