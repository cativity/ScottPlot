namespace ScottPlot.Panels;

public abstract class PanelBase : IPanel
{
    public bool IsVisible { get; set; } = true;

    public Edge Edge { get; set; }

    public bool ShowDebugInformation { get; set; }

    public float MinimumSize { get; set; } = 0;

    public float MaximumSize { get; set; } = float.MaxValue;

    public abstract float Measure();

    public abstract void Render(RenderPack rp, float size, float offset);

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
    {
        return Edge switch
        {
            Edge.Left => new PixelRect(dataRect.Left - size - offset, dataRect.Left - offset, dataRect.Bottom, dataRect.Top),
            Edge.Right => new PixelRect(dataRect.Right + offset, dataRect.Right + size + offset, dataRect.Bottom, dataRect.Top),
            Edge.Bottom => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Bottom + size + offset, dataRect.Bottom + offset),
            Edge.Top => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Top - offset, dataRect.Top - size - offset),
            _ => throw new NotImplementedException($"{Edge}")
        };
    }
}
