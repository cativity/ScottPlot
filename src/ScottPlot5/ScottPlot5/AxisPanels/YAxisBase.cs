namespace ScottPlot.AxisPanels;

public abstract class YAxisBase : AxisBase, IYAxis
{
    public double Height => Range.Span;

    //[Obsolete("use LabelText, LabelFontColor, LabelFontSize, LabelFontName, etc. or properties of LabelStyle", false)]
    public LabelStyle Label => LabelStyle;

    public float GetPixel(double position, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        double unitsFromMinValue = position - Min;
        float pxFromEdge = (float)(unitsFromMinValue * pxPerUnit);

        return dataArea.Bottom - pxFromEdge;
    }

    public double GetCoordinate(float pixel, PixelRect dataArea)
    {
        double pxPerUnit = dataArea.Height / Height;
        float pxFromMinValue = pixel - dataArea.Bottom;
        double unitsFromMinValue = pxFromMinValue / pxPerUnit;

        return Min - unitsFromMinValue;
    }

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
        float maxTickLabelWidth = TickGenerator?.Ticks.Length > 0 ? TickGenerator.Ticks.Max(x => TickLabelStyle.Measure(x.Label, paint).Width) : 0;

        float axisLabelHeight = string.IsNullOrEmpty(LabelStyle.Text)
                                    ? EmptyLabelPadding.Horizontal
                                    : LabelStyle.Measure(LabelText, paint).LineHeight
                                      + PaddingBetweenTickAndAxisLabels.Horizontal
                                      + PaddingOutsideAxisLabels.Horizontal;

        return maxTickLabelWidth + axisLabelHeight;
    }

    private static PixelRect GetPanelRectangleLeft(PixelRect dataRect, float size, float offset)
        => new PixelRect(dataRect.Left - offset - size, dataRect.Left - offset, dataRect.Bottom, dataRect.Top);

    private static PixelRect GetPanelRectangleRight(PixelRect dataRect, float size, float offset)
        => new PixelRect(dataRect.Right + offset, dataRect.Right + offset + size, dataRect.Bottom, dataRect.Top);

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
        => Edge == Edge.Left ? GetPanelRectangleLeft(dataRect, size, offset) : GetPanelRectangleRight(dataRect, size, offset);

    public virtual void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
        {
            return;
        }

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);
        float x = Edge == Edge.Left ? panelRect.Left + PaddingOutsideAxisLabels.Horizontal : panelRect.Right - PaddingOutsideAxisLabels.Horizontal;
        Pixel labelPoint = new Pixel(x, rp.DataRect.VerticalCenter);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, LabelFontColor);
        }

        using SKPaint paint = new SKPaint();
        LabelAlignment = Alignment.UpperCenter;
        LabelStyle.Render(rp.Canvas, labelPoint, paint);

        DrawTicks(rp, TickLabelStyle, panelRect, TickGenerator?.Ticks ?? [], this, MajorTickStyle, MinorTickStyle);
        DrawFrame(rp, panelRect, Edge, FrameLineStyle);
    }

    public double GetPixelDistance(double distance, PixelRect dataArea) => distance * dataArea.Height / Height;

    public double GetCoordinateDistance(float distance, PixelRect dataArea) => distance / (dataArea.Height / Height);

    public void RegenerateTicks(PixelLength size)
    {
        using SKPaint paint = new SKPaint();
        TickLabelStyle.ApplyToPaint(paint);
        TickGenerator?.Regenerate(Range.ToCoordinateRange, Edge, size, paint, TickLabelStyle);
    }
}
