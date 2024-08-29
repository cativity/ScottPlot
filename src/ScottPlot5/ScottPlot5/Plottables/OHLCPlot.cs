namespace ScottPlot.Plottables;

public class OHLCPlot(IOHLCSource data) : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    /// <summary>
    ///     X position of each symbol is sourced from the OHLC's DateTime by default.
    ///     If this option is enabled, X position will be an ascending integers starting at 0 with no gaps.
    /// </summary>
    public bool Sequential { get; set; }

    /// <summary>
    ///     Fractional width of the OHLC symbol relative to its time span
    /// </summary>
    public double SymbolWidth { get; set; } = .8;

    public LineStyle RisingStyle { get; } = new LineStyle { Color = Color.FromHex("#089981"), Width = 2, };

    public LineStyle FallingStyle { get; } = new LineStyle { Color = Color.FromHex("#f23645"), Width = 2, };

    public IEnumerable<LegendItem> LegendItems => [];

    public AxisLimits GetAxisLimits()
    {
        if (Sequential)
        {
            CoordinateRange yLimits = data.GetLimitsY();
            CoordinateRange xLimits = new CoordinateRange(0, data.GetOHLCs().Count - 1);

            return new AxisLimits(xLimits, yLimits);
        }

        AxisLimits limits = data.GetLimits();
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
        using SKPath risingPath = new SKPath();
        using SKPath fallingPath = new SKPath();

        IList<OHLC> ohlcs = data.GetOHLCs();

        for (int i = 0; i < ohlcs.Count; i++)
        {
            OHLC ohlc = ohlcs[i];
            bool isRising = ohlc.Close >= ohlc.Open;
            SKPath path = isRising ? risingPath : fallingPath;

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

            float open = Axes.GetPixelY(ohlc.Open);
            float close = Axes.GetPixelY(ohlc.Close);

            // do not render OHLCs off the screen
            if (right < rp.DataRect.Left || left > rp.DataRect.Right)
            {
                continue;
            }

            // center line
            path.MoveTo(center, top);
            path.LineTo(center, bottom);

            // left peg
            path.MoveTo(left, open);
            path.LineTo(center, open);

            // right peg
            path.MoveTo(center, close);
            path.LineTo(right, close);
        }

        RisingStyle.ApplyToPaint(paint);
        rp.Canvas.DrawPath(risingPath, paint);

        FallingStyle.ApplyToPaint(paint);
        rp.Canvas.DrawPath(fallingPath, paint);
    }
}
