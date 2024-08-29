using AvaKey = Avalonia.Input.Key;
using Avalonia.Input;
using Avalonia;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using Key = ScottPlot.Interactivity.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.Avalonia;

internal static class AvaPlotExtensions
{
    internal static Pixel ToPixel(this PointerEventArgs e, Visual visual)
    {
        float x = (float)e.GetPosition(visual).X;
        float y = (float)e.GetPosition(visual).Y;

        return new Pixel(x, y);
    }

    internal static void ProcessMouseDown(this UserInputProcessor processor, Pixel pixel, PointerUpdateKind kind)
    {
        IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonPressed => new LeftMouseDown(pixel),
            PointerUpdateKind.MiddleButtonPressed => new MiddleMouseDown(pixel),
            PointerUpdateKind.RightButtonPressed => new RightMouseDown(pixel),
            _ => new Unknown("mouse down", kind.ToString()),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseUp(this UserInputProcessor processor, Pixel pixel, PointerUpdateKind kind)
    {
        IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonReleased => new LeftMouseUp(pixel),
            PointerUpdateKind.MiddleButtonReleased => new MiddleMouseUp(pixel),
            PointerUpdateKind.RightButtonReleased => new RightMouseUp(pixel),
            _ => new Unknown("mouse up", kind.ToString()),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseMove(this UserInputProcessor processor, Pixel pixel)
    {
        processor.Process(new MouseMove(pixel));
    }

    internal static void ProcessMouseWheel(this UserInputProcessor processor, Pixel pixel, double delta)
    {
        IUserAction action = delta > 0 ? new MouseWheelUp(pixel) : new MouseWheelDown(pixel);

        processor.Process(action);
    }

    internal static void ProcessKeyDown(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = GetKey(e.Key);
        IUserAction action = new KeyDown(key);
        processor.Process(action);
    }

    internal static void ProcessKeyUp(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = GetKey(e.Key);
        IUserAction action = new KeyUp(key);
        processor.Process(action);
    }

    public static Key GetKey(AvaKey avaKey)
    {
        return avaKey switch
        {
            AvaKey.LeftAlt => StandardKeys.Alt,
            AvaKey.RightAlt => StandardKeys.Alt,
            AvaKey.LeftShift => StandardKeys.Shift,
            AvaKey.RightShift => StandardKeys.Shift,
            AvaKey.LeftCtrl => StandardKeys.Control,
            AvaKey.RightCtrl => StandardKeys.Control,
            _ => new Key(avaKey.ToString()),
        };
    }

    internal static Control.Key OldToKey(this KeyEventArgs e)
    {
        return e.Key switch
        {
            AvaKey.LeftAlt => Control.Key.Alt,
            AvaKey.RightAlt => Control.Key.Alt,
            AvaKey.LeftShift => Control.Key.Shift,
            AvaKey.RightShift => Control.Key.Shift,
            AvaKey.LeftCtrl => Control.Key.Ctrl,
            AvaKey.RightCtrl => Control.Key.Ctrl,
            _ => Control.Key.Unknown,
        };
    }

    internal static MouseButton OldToButton(this PointerUpdateKind kind)
    {
        return kind switch
        {
            PointerUpdateKind.LeftButtonPressed => MouseButton.Left,
            PointerUpdateKind.LeftButtonReleased => MouseButton.Left,

            PointerUpdateKind.RightButtonPressed => MouseButton.Right,
            PointerUpdateKind.RightButtonReleased => MouseButton.Right,

            PointerUpdateKind.MiddleButtonPressed => MouseButton.Middle,
            PointerUpdateKind.MiddleButtonReleased => MouseButton.Middle,

            _ => MouseButton.Unknown,
        };
    }
}
