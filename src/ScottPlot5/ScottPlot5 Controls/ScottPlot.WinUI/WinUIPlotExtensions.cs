using Windows.System;
using Windows.Foundation;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using Key = ScottPlot.Control.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.WinUI;

internal static class WinUIPlotExtensions
{
    internal static Pixel Pixel(this PointerRoutedEventArgs e, WinUIPlot plotControl)
    {
        Point position = e.GetCurrentPoint(plotControl).Position;
        position.X *= plotControl.DisplayScale;
        position.Y *= plotControl.DisplayScale;

        return position.ToPixel();
    }

    internal static Pixel ToPixel(this Point p) => new((float)p.X, (float)p.Y);

    internal static void ProcessMouseDown(this UserInputProcessor processor, WinUIPlot plotControl, PointerRoutedEventArgs e)
    {
        Pixel pixel = e.Pixel(plotControl);
        PointerUpdateKind kind = e.GetCurrentPoint(plotControl).Properties.PointerUpdateKind;

        IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonPressed => new LeftMouseDown(pixel),
            PointerUpdateKind.MiddleButtonPressed => new MiddleMouseDown(pixel),
            PointerUpdateKind.RightButtonPressed => new RightMouseDown(pixel),
            _ => new Unknown(kind.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseUp(this UserInputProcessor processor, WinUIPlot plotControl, PointerRoutedEventArgs e)
    {
        Pixel pixel = e.Pixel(plotControl);
        PointerUpdateKind kind = e.GetCurrentPoint(plotControl).Properties.PointerUpdateKind;

        IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonReleased => new LeftMouseUp(pixel),
            PointerUpdateKind.MiddleButtonReleased => new MiddleMouseUp(pixel),
            PointerUpdateKind.RightButtonReleased => new RightMouseUp(pixel),
            _ => new Unknown(kind.ToString(), "released"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseMove(this UserInputProcessor processor, WinUIPlot plotControl, PointerRoutedEventArgs e)
    {
        Pixel pixel = e.Pixel(plotControl);
        IUserAction action = new MouseMove(pixel);
        processor.Process(action);
    }

    internal static void ProcessMouseWheel(this UserInputProcessor processor, WinUIPlot plotControl, PointerRoutedEventArgs e)
    {
        Pixel pixel = e.Pixel(plotControl);

        IUserAction action = e.GetCurrentPoint(plotControl).Properties.MouseWheelDelta > 0 ? new MouseWheelUp(pixel) : new MouseWheelDown(pixel);

        processor.Process(action);
    }

    internal static void ProcessKeyDown(this UserInputProcessor processor, WinUIPlot plotControl, KeyRoutedEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        IUserAction action = new KeyDown(key);
        processor.Process(action);
    }

    internal static void ProcessKeyUp(this UserInputProcessor processor, WinUIPlot plotControl, KeyRoutedEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        IUserAction action = new KeyUp(key);
        processor.Process(action);
    }

    internal static MouseButton OldToButton(this PointerRoutedEventArgs e, WinUIPlot plotControl)
    {
        return e.GetCurrentPoint(plotControl).Properties.PointerUpdateKind switch
        {
            PointerUpdateKind.MiddleButtonPressed or PointerUpdateKind.MiddleButtonReleased => MouseButton.Middle,
            PointerUpdateKind.LeftButtonPressed or PointerUpdateKind.LeftButtonReleased => MouseButton.Left,
            PointerUpdateKind.RightButtonPressed or PointerUpdateKind.RightButtonReleased => MouseButton.Right,
            _ => MouseButton.Unknown,
        };
    }

    internal static Key OldToKey(this KeyRoutedEventArgs e)
    {
        return e.Key switch
        {
            VirtualKey.Control => Key.Ctrl,
            VirtualKey.LeftControl => Key.Ctrl,
            VirtualKey.RightControl => Key.Ctrl,

            VirtualKey.Menu => Key.Alt,
            VirtualKey.LeftMenu => Key.Alt,
            VirtualKey.RightMenu => Key.Alt,

            VirtualKey.Shift => Key.Shift,
            VirtualKey.LeftShift => Key.Shift,
            VirtualKey.RightShift => Key.Shift,

            _ => Key.Unknown,
        };
    }

    internal static Interactivity.Key ToKey(this KeyRoutedEventArgs e)
    {
        return e.Key switch
        {
            VirtualKey.Control => StandardKeys.Control,
            VirtualKey.LeftControl => StandardKeys.Control,
            VirtualKey.RightControl => StandardKeys.Control,

            VirtualKey.Menu => StandardKeys.Alt,
            VirtualKey.LeftMenu => StandardKeys.Alt,
            VirtualKey.RightMenu => StandardKeys.Alt,

            VirtualKey.Shift => StandardKeys.Shift,
            VirtualKey.LeftShift => StandardKeys.Shift,
            VirtualKey.RightShift => StandardKeys.Shift,
            _ => new Interactivity.Key(e.Key.ToString()),
        };
    }
}
