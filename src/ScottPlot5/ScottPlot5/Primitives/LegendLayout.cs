namespace ScottPlot;

public readonly struct LegendLayout
{
    public required LegendItem[] LegendItems { get; init; }

    public required PixelRect LegendRect { get; init; }

    public required PixelRect[] LabelRects { get; init; }

    public required PixelRect[] SymbolRects { get; init; }

    public static LegendLayout NoLegend => new LegendLayout { LegendItems = [], LegendRect = PixelRect.NaN, LabelRects = [], SymbolRects = [], };
}
