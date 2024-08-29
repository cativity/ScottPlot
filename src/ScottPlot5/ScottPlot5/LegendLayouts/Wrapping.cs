namespace ScottPlot.LegendLayouts;

public class Wrapping : ILegendLayout
{
    public LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize)
    {
        using SKPaint paint = new SKPaint();
        PixelSize maxSizeAfterPadding = maxSize.Contracted(legend.Padding);
        PixelRect maxRectAfterPadding = new PixelRect(0, maxSizeAfterPadding.Width, maxSizeAfterPadding.Height, 0);
        PixelSize[] labelSizes = items.Select(x => x.LabelStyle.Measure(x.LabelText, paint).Size).ToArray();
        float maxLabelWidth = labelSizes.Max(x => x.Width);
        float maxLabelHeight = labelSizes.Max(x => x.Height);
        float maxItemWidth = legend.SymbolWidth + legend.SymbolPadding + maxLabelWidth;

        PixelRect[] labelRects = new PixelRect[items.Length];
        PixelRect[] symbolRects = new PixelRect[items.Length];

        Pixel nextPixel = new Pixel(0, 0);

        for (int i = 0; i < items.Length; i++)
        {
            float itemWidth = legend.TightHorizontalWrapping ? legend.SymbolWidth + legend.SymbolPadding + labelSizes[i].Width : maxItemWidth;

            // if the next position will cause an overflow, wrap to the next position
            if (legend.Orientation == Orientation.Horizontal)
            {
                if (nextPixel.X + itemWidth > maxSizeAfterPadding.Width)
                {
                    nextPixel = new Pixel(0, nextPixel.Y + maxLabelHeight + legend.InterItemPadding.Bottom);
                }
            }
            else
            {
                if (nextPixel.Y + maxLabelHeight > maxSizeAfterPadding.Height)
                {
                    nextPixel = new Pixel(nextPixel.X + itemWidth + legend.InterItemPadding.Right, 0);
                }
            }

            // create rectangles for the item using the current position
            PixelRect itemRect = new PixelRect(nextPixel, new PixelSize(itemWidth, maxLabelHeight));
            itemRect = itemRect.Intersect(maxRectAfterPadding);

            symbolRects[i] = new PixelRect(itemRect.Left, itemRect.Left + legend.SymbolWidth, itemRect.Bottom, itemRect.Top);
            labelRects[i] = new PixelRect(itemRect.Left + legend.SymbolWidth + legend.SymbolPadding, itemRect.Right, itemRect.Bottom, itemRect.Top);

            // move the position forward according to the size of this item

            nextPixel = legend.Orientation == Orientation.Horizontal
                            ? new Pixel(nextPixel.X + itemWidth + legend.InterItemPadding.Right, nextPixel.Y)
                            : new Pixel(nextPixel.X, nextPixel.Y + maxLabelHeight + legend.InterItemPadding.Bottom);
        }

        float tightWidth = Math.Min(labelRects.Max(x => x.Right), maxSizeAfterPadding.Width);
        float tightHeight = Math.Min(labelRects.Max(x => x.Bottom), maxSizeAfterPadding.Height);
        PixelRect legendRect = new PixelRect(0, tightWidth + legend.Padding.Horizontal, tightHeight + legend.Padding.Vertical, 0);
        PixelOffset paddingOffset = new PixelOffset(legend.Padding.Left, legend.Padding.Top);

        return new LegendLayout
        {
            LegendItems = items,
            LegendRect = legendRect,
            LabelRects = labelRects.Select(x => x.WithOffset(paddingOffset)).ToArray(),
            SymbolRects = symbolRects.Select(x => x.WithOffset(paddingOffset)).ToArray(),
        };
    }
}
