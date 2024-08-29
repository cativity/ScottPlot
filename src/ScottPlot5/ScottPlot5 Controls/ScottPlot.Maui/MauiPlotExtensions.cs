using ScottPlot.Control;

namespace ScottPlot.Maui;

internal static class MauiPlotExtensions
{
    internal static Pixel Pixel(this TappedEventArgs e, MauiPlot plot)
    {
        Point? position = e.GetPosition(null);

        if (position is null)
        {
            return new Pixel(double.NaN, double.NaN);
        }

        Point tmpPos = new Point(position.Value.X * plot.DisplayScale, position.Value.X * plot.DisplayScale);

        return tmpPos.ToPixel();
    }

    internal static Pixel ToPixel(this Point p) => new((float)p.X, (float)p.Y);

    internal static Point ToPoint(this Pixel p) => new(p.X, p.Y);

    internal static MouseButton ToButton(this TappedEventArgs e, MauiPlot plot)
    {
        //Point? props = e.GetPosition(plot);

        return e.Buttons switch
        {
            ButtonsMask.Primary => MouseButton.Left,
            ButtonsMask.Secondary => MouseButton.Right,
            _ => MouseButton.Unknown,
        };
    }

    /*internal static Control.Key Key(this KeyRoutedEventArgs e)
    {
        return e.Key switch
        {
            VirtualKey.Control => Control.Key.Ctrl,
            VirtualKey.LeftControl => Control.Key.Ctrl,
            VirtualKey.RightControl => Control.Key.Ctrl,

            VirtualKey.Menu => Control.Key.Alt,
            VirtualKey.LeftMenu => Control.Key.Alt,
            VirtualKey.RightMenu => Control.Key.Alt,

            VirtualKey.Shift => Control.Key.Shift,
            VirtualKey.LeftShift => Control.Key.Shift,
            VirtualKey.RightShift => Control.Key.Shift,

            _ => Control.Key.Unknown,
        };
    }*/
}
