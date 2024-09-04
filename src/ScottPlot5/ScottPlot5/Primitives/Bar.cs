namespace ScottPlot;

/// <summary>
///     Represents a single bar in a bar chart
/// </summary>
public class Bar
{
    public double Position { get; set; }

    public double Value;

    public double ValueBase { get; set; }

    public double Error { get; set; }

    public bool IsVisible { get; set; } = true;

    public Color FillColor { get; set; } = Colors.Gray;

    public Color BorderColor { get; set; } = Colors.Black;

    public Color ErrorColor { get; set; } = Colors.Black;

    public double Size { get; set; } = 0.8; // coordinate units

    public double ErrorSize { get; set; } = 0.2; // coordinate units

    public float BorderLineWidth { get; set; } = 1;

    public float ErrorLineWidth { get; set; }

    // TODO: something like ErrorInDirectionOfValue?
    // Maybe ErrorPosition should be an enum containing: None, Upward, Downward, Both, or Extend
    public bool ErrorPositive { get; set; } = true;

    public bool ErrorNegative { get; set; } = true;

    public string Label { get; set; } = string.Empty;

    public bool CenterLabel { get; set; }

    public float LabelOffset { get; set; } = 5;

    public Orientation Orientation { get; set; } = Orientation.Vertical;

    internal CoordinateRect Rect
        => Orientation == Orientation.Vertical
               ? new CoordinateRect(Position - (Size / 2), Position + (Size / 2), ValueBase, Value)
               : new CoordinateRect(ValueBase, Value, Position - (Size / 2), Position + (Size / 2));

    internal IEnumerable<CoordinateLine> ErrorLines
    {
        get
        {
            CoordinateLine center;
            CoordinateLine top;
            CoordinateLine bottom;

            if (Orientation == Orientation.Vertical)
            {
                center = new CoordinateLine(Position, Value - Error, Position, Value + Error);
                top = new CoordinateLine(Position - (ErrorSize / 2), Value + Error, Position + (ErrorSize / 2), Value + Error);
                bottom = new CoordinateLine(Position - (ErrorSize / 2), Value - Error, Position + (ErrorSize / 2), Value - Error);
            }
            else
            {
                center = new CoordinateLine(Value - Error, Position, Value + Error, Position);
                top = new CoordinateLine(Value + Error, Position - (ErrorSize / 2), Value + Error, Position + (ErrorSize / 2));
                bottom = new CoordinateLine(Value - Error, Position - (ErrorSize / 2), Value - Error, Position + (ErrorSize / 2));
            }

            return [center, top, bottom];
        }
    }

    internal AxisLimits AxisLimits
        => Orientation == Orientation.Vertical
               ? new AxisLimits(Position - (Size / 2), Position + (Size / 2), Math.Min(ValueBase, Value - Error), Value + Error)
               : new AxisLimits(Math.Min(ValueBase, Value - Error), Value + Error, Position - (Size / 2), Position + (Size / 2));

    public void Render(RenderPack rp, IAxes axes, SKPaint paint, LabelStyle labelStyle)
    {
        if (!IsVisible)
        {
            return;
        }

        PixelRect rect = axes.GetPixelRect(Rect);
        Drawing.FillRectangle(rp.Canvas, rect, FillColor);
        Drawing.DrawRectangle(rp.Canvas, rect, BorderColor, BorderLineWidth);

        if (Error != 0)
        {
            foreach (CoordinateLine line in ErrorLines)
            {
                Pixel pt1 = axes.GetPixel(line.Start);
                Pixel pt2 = axes.GetPixel(line.End);
                Drawing.DrawLine(rp.Canvas, paint, pt1, pt2, BorderColor, BorderLineWidth);
            }
        }

        if (Orientation == Orientation.Vertical)
        {
            float xPx = rect.HorizontalCenter;
            float yPx = CenterLabel ? rect.VerticalCenter : rect.Top;
            labelStyle.Alignment = CenterLabel ? Alignment.MiddleCenter : Alignment.LowerCenter;
            Pixel labelPixel = new Pixel(xPx, yPx - LabelOffset);
            labelStyle.Render(rp.Canvas, labelPixel, paint);
        }
        else
        {
            MeasuredText measured = labelStyle.Measure(labelStyle.Text, paint);

            if (Value < 0)
            {
                float xPx = rect.LeftCenter.X - (LabelOffset + (measured.Width / 2));
                float yPx = rect.LeftCenter.Y + (measured.Height / 2);
                Pixel labelPixel = new Pixel(xPx, yPx);
                labelStyle.Render(rp.Canvas, labelPixel, paint);
            }
            else
            {
                float xPx = rect.RightCenter.X + (LabelOffset + (measured.Width / 2));
                float yPx = rect.RightCenter.Y + (measured.Height / 2);
                Pixel labelPixel = new Pixel(xPx, yPx);
                labelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }
    }
}
