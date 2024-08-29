namespace ScottPlot.Plottables;

/// <summary>
///     A vertical span marks the full horizontal range between two vertical values
/// </summary>
public class VerticalSpan : AxisSpan, IPlottable
{
    public double Y1 { get; set; }

    public double Y2 { get; set; }

    public CoordinateRange YRange => new CoordinateRange(Bottom, Top);

    public double Bottom
    {
        get => Math.Min(Y1, Y2);
        set => Y1 = value;
    }

    public double Top
    {
        get => Math.Max(Y1, Y2);
        set => Y2 = value;
    }

    public override AxisLimits GetAxisLimits() => EnableAutoscale ? AxisLimits.VerticalOnly(Bottom, Top) : AxisLimits.NoLimits;

    public override void Render(RenderPack rp)
    {
        PixelRangeY vert = new PixelRangeY(Axes.GetPixelY(Bottom), Axes.GetPixelY(Top));

        if (vert.Span < 1)
        {
            float middle = (vert.Top + vert.Bottom) / 2;
            vert = new PixelRangeY(middle - 0.5F, middle + 0.5F);
        }

        PixelRangeX horiz = new PixelRangeX(rp.DataRect.Left, rp.DataRect.Right);
        PixelRect rect = new PixelRect(horiz, vert);
        Render(rp, rect);
    }

    public override AxisSpanUnderMouse? UnderMouse(CoordinateRect rect)
    {
        AxisSpanUnderMouse spanUnderMouse = new AxisSpanUnderMouse { Span = this, MouseStart = rect.Center, OriginalRange = new CoordinateRange(Y1, Y2), };

        if (IsResizable)
        {
            if (rect.ContainsY(Y1))
            {
                spanUnderMouse.ResizeEdge1 = true;

                return spanUnderMouse;
            }

            if (rect.ContainsY(Y2))
            {
                spanUnderMouse.ResizeEdge2 = true;

                return spanUnderMouse;
            }
        }

        return IsDraggable && rect.YRange.Intersects(YRange) ? spanUnderMouse : null;
    }

    public override void DragTo(AxisSpanUnderMouse spanUnderMouse, Coordinates mouseNow)
    {
        if (spanUnderMouse.ResizeEdge1)
        {
            Y1 = mouseNow.Y;
        }
        else if (spanUnderMouse.ResizeEdge2)
        {
            Y2 = mouseNow.Y;
        }
        else
        {
            double dY = spanUnderMouse.MouseStart.Y - mouseNow.Y;
            Y1 = spanUnderMouse.OriginalRange.Min - dY;
            Y2 = spanUnderMouse.OriginalRange.Max - dY;
        }
    }
}
