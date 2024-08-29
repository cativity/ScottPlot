﻿using ScottPlot.LegendLayouts;

namespace ScottPlot;

public class Legend(Plot plot) : IPlottable, IHasOutline, IHasBackground, IHasShadow
{
    public Plot Plot { get; } = plot;

    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     Position of the legend relative to the data area
    /// </summary>
    public Alignment Alignment { get; set; } = Alignment.LowerRight;

    ///// <summary>
    /////     Position of the legend relative to the data area
    ///// </summary>
    //[Obsolete("use Alignment")]
    //public Alignment Location { get => Alignment; set => Alignment = value; }

    /// <summary>
    ///     Stack items in the legend according to this preferred orientation
    /// </summary>
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    ///     Distance from the edge of the data area to the edge of the legend
    /// </summary>
    public PixelPadding Margin { get; set; } = new PixelPadding(7, 7);

    /// <summary>
    ///     Distance between the legend frame and the items within it
    /// </summary>
    public PixelPadding Padding { get; set; } = new PixelPadding(10, 5);

    /// <summary>
    ///     Width of the symbol in a legend item
    /// </summary>
    public float SymbolWidth { get; set; } = 20;

    /// <summary>
    ///     Padding between a symbol and label within a legend item
    /// </summary>
    public float SymbolPadding { get; set; } = 5;

    /// <summary>
    ///     Space separating legend items
    /// </summary>
    public PixelPadding InterItemPadding { get; set; } = new PixelPadding(10, 10, 6, 6);

    /// <summary>
    ///     Items in this list will always be displayed in the legend
    /// </summary>
    public List<LegendItem> ManualItems { get; set; } = [];

    /// <summary>
    ///     If enabled, items in horizontal oriented legends will not
    ///     be aligned in columns but instead resized tightly to fit their contents
    /// </summary>
    public bool TightHorizontalWrapping { get; set; }

    public bool ShowItemRectanglesDebug { get; set; }

    /// <summary>
    ///     Enabling this allows multi-language text in the figure legend,
    ///     but may slow down the render loop.
    /// </summary>
    public bool SetBestFontOnEachRender { get; set; }

    /// <summary>
    ///     If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public float? FontSize { get; set; }

    /// <summary>
    ///     If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public string? FontName { get; set; }

    /// <summary>
    ///     If set, this overrides the value in the LegendItem's FontStyle
    /// </summary>
    public Color? FontColor { get; set; }

    //[Obsolete("Assign FontSize, FontName, or FontColor to control appearance of all legend items", true)]
    //public FillStyle Font { get; set; } = new FillStyle();

    //[Obsolete("Multiline is now enabled by default.", true)]
    //public bool AllowMultiline { get; set; }

    public ILegendLayout Layout { get; set; } = new Wrapping();

    public PixelSize LastRenderSize { get; private set; } = PixelSize.NaN;

    public LineStyle OutlineStyle { get; set; } = new LineStyle { Width = 1, Color = Colors.Black, };

    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }

    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }

    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }

    //[Obsolete("Assign BackgroundColor or interact with BackgroundFillStyle")]
    //public FillStyle BackgroundFill { get => BackgroundFillStyle; set { } }

    public FillStyle BackgroundFillStyle { get; } = new FillStyle { Color = Colors.White };

    public Color BackgroundColor { get => BackgroundFillStyle.Color; set => BackgroundFillStyle.Color = value; }

    public Color BackgroundHatchColor { get => BackgroundFillStyle.HatchColor; set => BackgroundFillStyle.HatchColor = value; }

    public IHatch? BackgroundHatch { get => BackgroundFillStyle.Hatch; set => BackgroundFillStyle.Hatch = value; }

    //[Obsolete("Assign ShadowColor or interact with ShadowFillStyle")]
    //public FillStyle ShadowFill { get => ShadowFillStyle; set { } }

    public FillStyle ShadowFillStyle { get; } = new FillStyle { Color = Colors.Black.WithOpacity(.2) };

    public Color ShadowColor { get => ShadowFillStyle.Color; set => ShadowFillStyle.Color = value; }

    public PixelOffset ShadowOffset { get; set; } = new PixelOffset(3, 3);

    public Alignment ShadowAlignment { get; set; } = Alignment.LowerRight;

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public bool DisplayPlottableLegendItems { get; set; } = true;

    public LegendItem[] GetItems()
    {
        List<LegendItem> items = [];

        if (DisplayPlottableLegendItems)
        {
            IEnumerable<LegendItem> plottableLegendItems = Plot.PlottableList.Where(static item => item.IsVisible)
                                                               .SelectMany(static x => x.LegendItems)
                                                               .Where(static x => !string.IsNullOrEmpty(x.LabelText));

            items.AddRange(plottableLegendItems);
        }

        items.AddRange(ManualItems);

        items = items.Where(static x => x.IsVisible).ToList();

        if (SetBestFontOnEachRender)
        {
            foreach (LegendItem item in items)
            {
                item.LabelStyle.SetBestFont();
            }
        }

        foreach (LegendItem item in items)
        {
            if (FontSize is not null)
            {
                item.LabelFontSize = FontSize.Value;
            }

            if (FontName is not null)
            {
                item.LabelFontName = FontName;
            }

            if (FontColor is not null)
            {
                item.LabelFontColor = FontColor.Value;
            }
        }

        return [.. items];
    }

    /// <summary>
    ///     Return an Image containing just the legend
    /// </summary>
    public Image GetImage()
    {
        LegendItem[] items = GetItems();

        LegendLayout lp = items.Length > 0 ? Layout.GetLayout(this, items, PixelSize.Infinity) : LegendLayout.NoLegend;

        SKImageInfo info = new SKImageInfo(Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Width)),
                                           Math.Max(1, (int)Math.Ceiling(lp.LegendRect.Height)),
                                           SKColorType.Rgba8888,
                                           SKAlphaType.Premul);

        using SKSurface surface = SKSurface.Create(info) ?? throw new NullReferenceException("invalid SKImageInfo");

        RenderLayout(surface.Canvas, lp);

        return new Image(surface);
    }

    /// <summary>
    ///     Return contents of a SVG image containing just the legend
    /// </summary>
    /// <returns></returns>
    public string GetSvgXml()
    {
        LegendItem[] items = GetItems();

        LegendLayout lp = items.Length > 0 ? Layout.GetLayout(this, items, PixelSize.Infinity) : LegendLayout.NoLegend;

        int width = (int)Math.Ceiling(lp.LegendRect.Width);
        int height = (int)Math.Ceiling(lp.LegendRect.Height);
        using SvgImage svg = new SvgImage(width, height);
        RenderLayout(svg.Canvas, lp);

        return svg.GetXml();
    }

    /// <summary>
    ///     This is called automatically by the render manager
    /// </summary>
    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
        {
            return;
        }

        LegendItem[] items = GetItems();

        if (items.Length == 0)
        {
            return;
        }

        PixelRect dataRectAfterMargin = rp.DataRect.Contract(Margin);
        LegendLayout tightLayout = Layout.GetLayout(this, items, dataRectAfterMargin.Size);
        PixelRect standaloneLegendRect = tightLayout.LegendRect.AlignedInside(dataRectAfterMargin, Alignment);
        PixelOffset legendOffset = new PixelOffset(standaloneLegendRect.Left, standaloneLegendRect.Top);

        LegendLayout layout = new LegendLayout
        {
            LegendItems = tightLayout.LegendItems,
            LegendRect = tightLayout.LegendRect.WithOffset(legendOffset),
            LabelRects = tightLayout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = tightLayout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        RenderLayout(rp.Canvas, layout);
    }

    /// <summary>
    ///     Render the legend inside the given rectangle
    /// </summary>
    /// <param name="rp"></param>
    public void Render(RenderPack rp, PixelRect rect, Alignment alignment)
    {
        LegendItem[] items = GetItems();

        if (items.Length == 0)
        {
            return;
        }

        LegendLayout tightLayout = Layout.GetLayout(this, items, rect.Size);
        PixelRect standaloneLegendRect = tightLayout.LegendRect.AlignedInside(rect, alignment);
        PixelOffset legendOffset = new PixelOffset(standaloneLegendRect.Left, standaloneLegendRect.Top);

        LegendLayout layout = new LegendLayout
        {
            LegendItems = tightLayout.LegendItems,
            LegendRect = tightLayout.LegendRect.WithOffset(legendOffset),
            LabelRects = tightLayout.LabelRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
            SymbolRects = tightLayout.SymbolRects.Select(x => x.WithOffset(legendOffset)).ToArray(),
        };

        RenderLayout(rp.Canvas, layout);
    }

    private void RenderLayout(SKCanvas canvas, LegendLayout layout)
    {
        LastRenderSize = layout.LegendRect.Size;

        using SKPaint paint = new SKPaint();

        // render the legend panel
        PixelRect shadowRect = layout.LegendRect.WithOffset(ShadowOffset);
        Drawing.FillRectangle(canvas, shadowRect, paint, ShadowFillStyle);
        Drawing.FillRectangle(canvas, layout.LegendRect, paint, BackgroundFillStyle);
        Drawing.DrawRectangle(canvas, layout.LegendRect, paint, OutlineStyle);

        CanvasState canvasState = new CanvasState(canvas);
        canvasState.Save();

        PixelRect clipRect = layout.LegendRect.Contract(Padding).Expand(1.0f);

        canvasState.Clip(clipRect);

        // render items inside the legend
        for (int i = 0; i < layout.LegendItems.Length; i++)
        {
            LegendItem item = layout.LegendItems[i];
            PixelRect labelRect = layout.LabelRects[i];
            PixelRect symbolRect = layout.SymbolRects[i];
            PixelRect symbolFillRect = symbolRect.Contract(0, symbolRect.Height * .2f);
            PixelRect symbolFillOutlineRect = symbolFillRect.Expand(1 - item.OutlineWidth);
            PixelLine symbolLine = new PixelLine(symbolRect.RightCenter, symbolRect.LeftCenter);

            item.LabelStyle.Render(canvas, labelRect.LeftCenter, paint, true);

            if (ShowItemRectanglesDebug)
            {
                Drawing.DrawRectangle(canvas, symbolRect, Colors.Magenta.WithAlpha(.2));
                Drawing.DrawRectangle(canvas, labelRect, Colors.Magenta.WithAlpha(.2));
            }

            item.LineStyle.Render(canvas, symbolLine, paint);
            item.FillStyle.Render(canvas, symbolFillRect, paint);
            item.OutlineStyle.Render(canvas, symbolFillOutlineRect, paint);
            item.MarkerStyle.Render(canvas, symbolRect.Center, paint);
            item.ArrowStyle.Render(canvas, symbolLine, paint);
        }

        canvasState.Restore();
    }
}
