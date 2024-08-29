using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using Key = ScottPlot.Control.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.WPF;

internal static class WpfPlotExtensions
{
    internal static Pixel ToPixel(this MouseEventArgs e, FrameworkElement fe) => fe.ToPixel(e.GetPosition(fe));

    internal static Pixel ToPixel(this FrameworkElement fe, Point position)
    {
        DpiScale dpiScale = VisualTreeHelper.GetDpi(fe);

        return new Pixel((float)(position.X * dpiScale.DpiScaleX), (float)(position.Y * dpiScale.DpiScaleY));
    }

    internal static void ProcessMouseDown(this UserInputProcessor processor, FrameworkElement fe, MouseButtonEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        IUserAction action = e.ChangedButton switch
        {
            System.Windows.Input.MouseButton.Left => new LeftMouseDown(pixel),
            System.Windows.Input.MouseButton.Middle => new MiddleMouseDown(pixel),
            System.Windows.Input.MouseButton.Right => new RightMouseDown(pixel),
            _ => new Unknown(e.ChangedButton.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseUp(this UserInputProcessor processor, FrameworkElement fe, MouseButtonEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        IUserAction action = e.ChangedButton switch
        {
            System.Windows.Input.MouseButton.Left => new LeftMouseUp(pixel),
            System.Windows.Input.MouseButton.Middle => new MiddleMouseUp(pixel),
            System.Windows.Input.MouseButton.Right => new RightMouseUp(pixel),
            _ => new Unknown(e.ChangedButton.ToString(), "released"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseMove(this UserInputProcessor processor, FrameworkElement fe, MouseEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);
        IUserAction action = new MouseMove(pixel);
        processor.Process(action);
    }

    internal static void ProcessMouseWheel(this UserInputProcessor processor, FrameworkElement fe, MouseWheelEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        IUserAction action = e.Delta > 0 ? new MouseWheelUp(pixel) : new MouseWheelDown(pixel);

        processor.Process(action);
    }

    internal static void ProcessKeyDown(this UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        IUserAction action = new KeyDown(key);
        processor.Process(action);
    }

    internal static void ProcessKeyUp(this UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        IUserAction action = new KeyUp(key);
        processor.Process(action);
    }

    internal static MouseButton OldToButton(this MouseButtonEventArgs e)
    {
        return e.ChangedButton switch
        {
            System.Windows.Input.MouseButton.Middle => MouseButton.Middle,
            System.Windows.Input.MouseButton.Left => MouseButton.Left,
            System.Windows.Input.MouseButton.Right => MouseButton.Right,
            _ => MouseButton.Unknown
        };
    }

    internal static Key OldToKey(this KeyEventArgs e)
    {
        System.Windows.Input.Key key = e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key; // required to capture Alt

        return key switch
        {
            System.Windows.Input.Key.LeftCtrl => Key.Ctrl,
            System.Windows.Input.Key.RightCtrl => Key.Ctrl,
            System.Windows.Input.Key.LeftAlt => Key.Alt,
            System.Windows.Input.Key.RightAlt => Key.Alt,
            System.Windows.Input.Key.LeftShift => Key.Shift,
            System.Windows.Input.Key.RightShift => Key.Shift,
            _ => Key.Unknown,
        };
    }

    internal static Interactivity.Key ToKey(this KeyEventArgs e)
    {
        System.Windows.Input.Key key = e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key; // required to capture Alt

        return key switch
        {
            System.Windows.Input.Key.LeftCtrl => StandardKeys.Control,
            System.Windows.Input.Key.RightCtrl => StandardKeys.Control,
            System.Windows.Input.Key.LeftAlt => StandardKeys.Alt,
            System.Windows.Input.Key.RightAlt => StandardKeys.Alt,
            System.Windows.Input.Key.LeftShift => StandardKeys.Shift,
            System.Windows.Input.Key.RightShift => StandardKeys.Shift,
            _ => new Interactivity.Key(key.ToString()),
        };
    }

    internal static BitmapImage GetBitmapImage(this Plot plot, int width, int height)
    {
        byte[] bytes = plot.GetImageBytes(width, height);
        using MemoryStream ms = new MemoryStream(bytes);

        BitmapImage bmp = new BitmapImage();
        bmp.BeginInit();
        bmp.StreamSource = ms;
        bmp.EndInit();
        bmp.Freeze();

        return bmp;
    }
}
