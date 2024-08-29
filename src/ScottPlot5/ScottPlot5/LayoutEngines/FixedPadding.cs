namespace ScottPlot.LayoutEngines;

/// <summary>
///     Generate layouts where the data area has a fixed padding from the edge of the figure
/// </summary>
public class FixedPadding(PixelPadding padding) : ILayoutEngine
{
    private PixelPadding Padding { get; } = padding;

    public Layout GetLayout(PixelRect figureRect, Plot plot)
    {
        List<IPanel> panels = [.. plot.Axes.GetPanels()];

        // must recalculate ticks before measuring panels

        // NOTE: the actual ticks will be regenerated later, after the layout is determined
        panels.OfType<IXAxis>().ToList().ForEach(x => x.RegenerateTicks(figureRect.Width));
        panels.OfType<IYAxis>().ToList().ForEach(x => x.RegenerateTicks(figureRect.Height));

        Dictionary<IPanel, float> panelSizes = LayoutEngineBase.MeasurePanels(panels);
        Dictionary<IPanel, float> panelOffsets = LayoutEngineBase.GetPanelOffsets(panels, panelSizes);

        PixelRect dataRect = new PixelRect(figureRect.Left + Padding.Left,
                                           figureRect.Left + figureRect.Width - Padding.Right,
                                           figureRect.Top + figureRect.Height - Padding.Bottom,
                                           figureRect.Top + Padding.Top);

        return new Layout(figureRect, dataRect, panelSizes, panelOffsets);
    }
}
