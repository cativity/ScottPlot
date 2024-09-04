namespace ScottPlot.Plottables;

public abstract class AxisLine : LabelStyleProperties, IPlottable, IRenderLast, IHasLine, IHasLegendText
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; set; } = new LineStyle { Width = 1 };

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }

    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public override LabelStyle LabelStyle { get; set; } = new LabelStyle();

    public string Text { get => LabelText; set => LabelText = value; }

    public string LegendText { get; set; } = string.Empty;

    public Alignment? ManualLabelAlignment { get; set; }

    //#region obsolete

    //[Obsolete("Use ManualLabelAlignment")]
    //public Alignment? TextAlignment { get => ManualLabelAlignment; set => ManualLabelAlignment = value; }

    //[Obsolete("Set LabelFontSize, LabelBold, LabelFontColor, or properties of the LabelStyle object.")]
    //public LabelStyle Label { get => LabelStyle; set => LabelStyle = value; }

    //[Obsolete("Use LabelFontSize")]
    //public float FontSize { get => LabelFontSize; set => LabelFontSize = value; }

    //[Obsolete("Use LabelBold")]
    //public bool FontBold { get => LabelBold; set => LabelBold = value; }

    //[Obsolete("Use LabelFontName")]
    //public string FontName { get => LabelFontName; set => LabelFontName = value; }

    //[Obsolete("Use LabelFontColor")]
    //public Color FontColor { get => LabelFontColor; set => LabelFontColor = value; }

    //[Obsolete("Use LabelFontColor")]
    //public Color TextColor { get => LabelFontColor; set => LabelFontColor = value; }

    //[Obsolete("Use LabelFontColor")]
    //public Color TextBackgroundColor { get => LabelFontColor; set => LabelFontColor = value; }

    //[Obsolete("Use LabelRotation")]
    //public float TextRotation { get => LabelRotation; set => LabelRotation = value; }

    //[Obsolete("Use LabelFontSize")]
    //public float TextSize { get => LabelFontSize; set => LabelFontSize = value; }

    //#endregion

    public bool LabelOppositeAxis { get; set; }

    public bool IsDraggable { get; set; }

    public bool ExcludeFromLegend { get; set; }

    public bool EnableAutoscale { get; set; } = true;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            LabelStyle.BackgroundColor = value;
        }
    }

    public double Position { get; set; }

    public IEnumerable<LegendItem> LegendItems
        => LegendItem.Single(new LegendItem
        {
            LabelText = ExcludeFromLegend ? string.Empty : LegendText,
            LineStyle = LineStyle,
            MarkerStyle = MarkerStyle.None,
        });

    public abstract bool IsUnderMouse(CoordinateRect rect);

    public abstract AxisLimits GetAxisLimits();

    public abstract void Render(RenderPack rp);

    public abstract void RenderLast(RenderPack rp);
}
