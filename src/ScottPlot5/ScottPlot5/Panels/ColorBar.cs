using ScottPlot.AxisPanels;

namespace ScottPlot.Panels;

/// <summary>
///     An axis panel which displays a colormap and range of values
/// </summary>
public class ColorBar(IHasColorAxis source, Edge edge = Edge.Right) : IPanel
{
    public bool IsVisible { get; set; } = true;

    public IHasColorAxis Source { get; set; } = source;

    public Edge Edge { get; set; } = edge;

    /// <summary>
    ///     Axis (spine, ticks, label, etc) for the colorbar
    /// </summary>
    public IAxis Axis { get; } = edge switch
    {
        Edge.Left => new LeftAxis(),
        Edge.Right => new RightAxis(),
        Edge.Bottom => new BottomAxis(),
        Edge.Top => new TopAxis(),
        _ => throw new NotImplementedException()
    };

    /// <summary>
    ///     Thickness of the colorbar image (in pixels)
    /// </summary>
    public float Width { get; set; } = 30;

    /// <summary>
    ///     Title for the colorbar, displayed outside the ticks.
    /// </summary>
    public string Label
    {
        get => Axis.Label.Text;
        set => Axis.Label.Text = value;
    }

    /// <summary>
    ///     Title for the colorbar, displayed outside the ticks.
    /// </summary>
    public LabelStyle LabelStyle => Axis.Label;

    public bool ShowDebugInformation { get; set; }

    public float MinimumSize { get; set; } = 0;

    public float MaximumSize { get; set; } = float.MaxValue;

    public float Measure()
    {
        if (!IsVisible)
        {
            return 0;
        }

        // use an example DataRect to estimate the size required by the ticks
        PixelRect guessedDataArea = new PixelRect(0, 600, 400, 0);
        GenerateTicks(guessedDataArea);
        float guessedAxisSize = Axis.Measure();

        return Width + guessedAxisSize;
    }

    /// <summary>
    ///     Return a rectangle encapsulating the colormap
    ///     bitmap plus the axis and ticks.
    /// </summary>
    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        PixelRect bmpRect = GetColormapBitmapRect(dataRect, size, offset);
        GenerateTicks(dataRect);
        float axisSize = Axis.Measure();

        return Edge switch
        {
            Edge.Left => bmpRect.ExpandX(bmpRect.Left - axisSize),
            Edge.Right => bmpRect.ExpandX(bmpRect.Right + axisSize),
            Edge.Bottom => bmpRect.ExpandY(bmpRect.Bottom + axisSize),
            Edge.Top => bmpRect.ExpandY(bmpRect.Top - axisSize),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    ///     Return the rectangle to the side of the data area
    ///     where the colormap bitmap will be drawn.
    /// </summary>
    private PixelRect GetColormapBitmapRect(PixelRect dataRect, float size, float offset)
    {
        // TODO: use size too
        return Edge switch
        {
            Edge.Left => new PixelRect(dataRect.Left - Width - offset, dataRect.Left - offset, dataRect.Bottom, dataRect.Top),
            Edge.Right => new PixelRect(dataRect.Right + offset, dataRect.Right + Width + offset, dataRect.Bottom, dataRect.Top),
            Edge.Bottom => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Bottom + Width + offset, dataRect.Bottom + offset),
            Edge.Top => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Top - offset, dataRect.Top - Width - offset),
            _ => throw new NotImplementedException($"{Edge}")
        };
    }

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
        {
            return;
        }

        PixelRect colormapRect = GetColormapBitmapRect(rp.DataRect, size, offset);
        RenderColorbarBitmap(rp, colormapRect);
        RenderColorbarAxis(rp, colormapRect, size, offset);
    }

    private void RenderColorbarBitmap(RenderPack rp, PixelRect colormapRect)
    {
        using SKBitmap bmp = Source.Colormap.GetSKBitmap(Edge.IsVertical());
        rp.Canvas.DrawBitmap(bmp, colormapRect.ToSKRect());
    }

    private void RenderColorbarAxis(RenderPack rp, PixelRect colormapRect, float size, float offset)
    {
        GenerateTicks(rp.DataRect);

        float size2 = Edge switch
        {
            Edge.Left => size - colormapRect.Width,
            Edge.Right => size - colormapRect.Width,
            Edge.Bottom => size - colormapRect.Height,
            Edge.Top => size - colormapRect.Height,
            _ => throw new NotImplementedException(),
        };

        float offset2 = Edge switch
        {
            Edge.Left => rp.DataRect.Left - colormapRect.Left,
            Edge.Right => colormapRect.Right - rp.DataRect.Right,
            Edge.Bottom => colormapRect.Bottom - rp.DataRect.Bottom,
            Edge.Top => rp.DataRect.Top - colormapRect.Top,
            _ => throw new NotImplementedException(),
        };

        Axis.Render(rp, size2, offset2);
    }

    private void GenerateTicks(PixelRect dataRect)
    {
        Range range = Source.GetRange();
        Axis.Min = range.Min;
        Axis.Max = range.Max;

        float edgeLength = Edge.IsVertical() ? dataRect.Height : dataRect.Width;

        Axis.RegenerateTicks(edgeLength);
    }
}
