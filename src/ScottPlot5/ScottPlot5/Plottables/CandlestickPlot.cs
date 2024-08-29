namespace ScottPlot.Plottables;

public class CandlestickPlot(IOHLCSource data) : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    /// <summary>
    ///     X position of candles is sourced from the OHLC's DateTime by default.
    ///     If this option is enabled, X position will be an ascending integers starting at 0 with no gaps.
    /// </summary>
    public bool Sequential { get; set; }

    /// <summary>
    ///     Fractional width of the candle symbol relative to its time span
    /// </summary>
    public double SymbolWidth { get; set; } = .8;

    public LineStyle RisingLineStyle { get; } = new LineStyle { Color = Color.FromHex("#089981"), Width = 2, };

    public LineStyle FallingLineStyle { get; } = new LineStyle { Color = Color.FromHex("#f23645"), Width = 2, };

    public FillStyle RisingFillStyle { get; } = new FillStyle { Color = Color.FromHex("#089981"), };

    public FillStyle FallingFillStyle { get; } = new FillStyle { Color = Color.FromHex("#f23645"), };

    public IEnumerable<LegendItem> LegendItems => [];

    public AxisLimits GetAxisLimits()
    {
        AxisLimits limits = data.GetLimits(); // TODO: Data.GetSequentialLimits()

        if (Sequential)
        {
            return new AxisLimits(0, data.GetOHLCs().Count, limits.Bottom, limits.Top);
        }

        List<OHLC> ohlcs = data.GetOHLCs();

        if (ohlcs.Count == 0)
        {
            return limits;
        }

        double left = ohlcs[0].DateTime.ToOADate() - (ohlcs[0].TimeSpan.TotalDays / 2);
        double right = ohlcs[^1].DateTime.ToOADate() + (ohlcs[^1].TimeSpan.TotalDays / 2);

        return new AxisLimits(left, right, limits.Bottom, limits.Top);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new SKPaint();

        IList<OHLC> ohlcs = data.GetOHLCs();

        for (int i = 0; i < ohlcs.Count; i++)
        {
            OHLC ohlc = ohlcs[i];
            bool isRising = ohlc.Close >= ohlc.Open;
            LineStyle lineStyle = isRising ? RisingLineStyle : FallingLineStyle;
            FillStyle fillStyle = isRising ? RisingFillStyle : FallingFillStyle;

            float top = Axes.GetPixelY(ohlc.High);
            float bottom = Axes.GetPixelY(ohlc.Low);

            float center,
                  left,
                  right;

            if (!Sequential)
            {
                double centerNumber = NumericConversion.ToNumber(ohlc.DateTime);
                center = Axes.GetPixelX(centerNumber);
                double halfWidthNumber = ohlc.TimeSpan.TotalDays / 2 * SymbolWidth;
                left = Axes.GetPixelX(centerNumber - halfWidthNumber);
                right = Axes.GetPixelX(centerNumber + halfWidthNumber);
            }
            else
            {
                center = Axes.GetPixelX(i);
                left = Axes.GetPixelX(i - ((float)SymbolWidth / 2));
                right = Axes.GetPixelX(i + ((float)SymbolWidth / 2));
            }

            // do not render OHLCs off the screen
            if (right < rp.DataRect.Left || left > rp.DataRect.Right)
            {
                continue;
            }

            float open = Axes.GetPixelY(ohlc.Open);
            float close = Axes.GetPixelY(ohlc.Close);

            // center line
            using SKPath path = new SKPath();
            path.MoveTo(center, top);
            path.LineTo(center, bottom);

            lineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            // rectangle
            SKRect rect = new SKRect(left, Math.Max(open, close), right, Math.Min(open, close));

            if (open != close)
            {
                fillStyle.ApplyToPaint(paint, rect.ToPixelRect());
                rp.Canvas.DrawRect(rect, paint);
            }
            else
            {
                lineStyle.ApplyToPaint(paint);
                rp.Canvas.DrawLine(left, open, right, open, paint);
            }
        }
    }
}
