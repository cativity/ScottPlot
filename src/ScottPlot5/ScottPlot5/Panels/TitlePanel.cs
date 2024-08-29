namespace ScottPlot.Panels;

public class TitlePanel : IPanel
{
    public bool IsVisible { get; set; } = true;

    public Edge Edge => Edge.Top;

    public bool ShowDebugInformation { get; set; }

    public float MinimumSize { get; set; } = 0;

    public float MaximumSize { get; set; } = float.MaxValue;

    public TitlePanel() => Label.Rotation = 0;

    public LabelStyle Label { get; } = new LabelStyle { Text = string.Empty, FontSize = 16, Bold = true, Alignment = Alignment.LowerCenter, };

    /// <summary>
    ///     Extra space to add above the title text so the title does not touch the edge of the image
    /// </summary>
    public float VerticalPadding { get; set; } = 10;

    public PixelRect GetPanelRect(PixelRect dataRect, float size, float offset)
        => new PixelRect(dataRect.Left, dataRect.Right, dataRect.Top - offset, dataRect.Top - offset - size);

    public float Measure()
    {
        if (!IsVisible)
        {
            return 0;
        }

        if (string.IsNullOrWhiteSpace(Label.Text))
        {
            return 0;
        }

        using SKPaint paint = new SKPaint();

        return Label.Measure(Label.Text, paint).Height + VerticalPadding;
    }

    public void Render(RenderPack rp, float size, float offset)
    {
        if (!IsVisible)
        {
            return;
        }

        using SKPaint paint = new SKPaint();

        PixelRect panelRect = GetPanelRect(rp.DataRect, size, offset);

        Pixel labelPoint = new Pixel(panelRect.HorizontalCenter, panelRect.Bottom);

        if (ShowDebugInformation)
        {
            Drawing.DrawDebugRectangle(rp.Canvas, panelRect, labelPoint, Label.ForeColor);
        }

        Label.Render(rp.Canvas, labelPoint, paint);
    }
}
