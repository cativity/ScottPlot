using Eto.Forms;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.Eto;

internal static class EtoPlotExtensions
{
    public static void ProcessMouseDown(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new LeftMouseDown(pixel),
            MouseButtons.Middle => new MiddleMouseDown(pixel),
            MouseButtons.Alternate => new RightMouseDown(pixel),
            _ => new Unknown(e.Buttons.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new LeftMouseUp(pixel),
            MouseButtons.Middle => new MiddleMouseUp(pixel),
            MouseButtons.Alternate => new RightMouseUp(pixel),
            _ => new Unknown(e.Buttons.ToString(), "released"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseMove(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();
        IUserAction action = new MouseMove(pixel);
        processor.Process(action);
    }

    public static void ProcessMouseWheel(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        IUserAction action = e.Delta.Height > 0 ? new MouseWheelUp(pixel) : new MouseWheelDown(pixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = e.ToKey();
        IUserAction action = new KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = e.ToKey();
        IUserAction action = new KeyUp(key);
        processor.Process(action);
    }

    public static Key ToKey(this KeyEventArgs e)
    {
        return e.Key switch
        {
            Keys.LeftControl => StandardKeys.Control,
            Keys.RightControl => StandardKeys.Control,
            Keys.LeftAlt => StandardKeys.Alt,
            Keys.RightAlt => StandardKeys.Alt,
            Keys.LeftShift => StandardKeys.Shift,
            Keys.RightShift => StandardKeys.Shift,
            _ => new Key(e.ToString() ?? string.Empty),
        };
    }

    internal static Pixel Pixel(this MouseEventArgs e)
    {
        double x = e.Location.X;
        double y = e.Location.Y;

        return new Pixel((float)x, (float)y);
    }

    internal static MouseButton OldToButton(this MouseEventArgs e)
    {
        return e.Buttons switch
        {
            MouseButtons.Middle => MouseButton.Middle,
            MouseButtons.Primary => MouseButton.Left,
            MouseButtons.Alternate => MouseButton.Right,
            _ => MouseButton.Unknown
        };
    }

    internal static Control.Key OldToKey(this KeyEventArgs e)
    {
        return e.Key switch
        {
            Keys.LeftControl => Control.Key.Ctrl,
            Keys.RightControl => Control.Key.Ctrl,
            Keys.LeftAlt => Control.Key.Alt,
            Keys.RightAlt => Control.Key.Alt,
            Keys.LeftShift => Control.Key.Shift,
            Keys.RightShift => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }
}
