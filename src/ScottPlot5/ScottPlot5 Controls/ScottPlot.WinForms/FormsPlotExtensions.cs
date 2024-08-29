using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;
using Key = ScottPlot.Interactivity.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.WinForms;

public static class FormsPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e) => new Pixel(e.X, e.Y);

    public static void ProcessMouseDown(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.X, e.Y);

        IUserAction action = e.Button switch
        {
            MouseButtons.Left => new LeftMouseDown(mousePixel),
            MouseButtons.Right => new RightMouseDown(mousePixel),
            MouseButtons.Middle => new MiddleMouseDown(mousePixel),
            _ => new Unknown("mouse button", e.ToString()),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.X, e.Y);

        IUserAction action = e.Button switch
        {
            MouseButtons.Left => new LeftMouseUp(mousePixel),
            MouseButtons.Right => new RightMouseUp(mousePixel),
            MouseButtons.Middle => new MiddleMouseUp(mousePixel),
            _ => new Unknown("mouse button", e.ToString()),
        };

        processor.Process(action);
    }

    public static void ProcessMouseMove(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.X, e.Y);
        IUserAction action = new MouseMove(mousePixel);
        processor.Process(action);
    }

    //[Obsolete("Double-clicks do not require processing. They are inferred from delay between single clicks.", true)]
    //public static void ProcessDoubleClick(this UserInputProcessor processor, EventArgs e)
    //{
    //}

    public static void ProcessMouseWheel(this UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new Pixel(e.X, e.Y);

        IUserAction action = e.Delta > 0 ? new MouseWheelUp(mousePixel) : new MouseWheelDown(mousePixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = e.GetKey();
        IUserAction action = new KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this UserInputProcessor processor, KeyEventArgs e)
    {
        Key key = e.GetKey();
        IUserAction action = new KeyUp(key);
        processor.Process(action);
    }

    internal static Key GetKey(this KeyEventArgs e) => GetKey(e.KeyCode);

    public static Key GetKey(this Keys keys)
    {
        // strip modifiers
        Keys keyCode = keys & ~Keys.Modifiers;

        Key key = keyCode switch
        {
            Keys.Alt => StandardKeys.Alt,
            Keys.Menu => StandardKeys.Alt,
            Keys.Shift => StandardKeys.Shift,
            Keys.ShiftKey => StandardKeys.Shift,
            Keys.LShiftKey => StandardKeys.Shift,
            Keys.RShiftKey => StandardKeys.Shift,
            Keys.Control => StandardKeys.Control,
            Keys.ControlKey => StandardKeys.Control,
            Keys.Down => StandardKeys.Down,
            Keys.Up => StandardKeys.Up,
            Keys.Left => StandardKeys.Left,
            Keys.Right => StandardKeys.Right,
            _ => StandardKeys.Unknown,
        };

        if (key != StandardKeys.Unknown)
        {
            return key;
        }

        string keyName = keyCode.ToString();

        return keyName.Length == 1 ? new Key(keyName) : new Key($"Unknown modifier key {keyName}");
    }

    internal static MouseButton Button(this MouseEventArgs e)
    {
        return e.Button switch
        {
            MouseButtons.Left => MouseButton.Left,
            MouseButtons.Right => MouseButton.Right,
            MouseButtons.Middle => MouseButton.Middle,
            _ => MouseButton.Unknown,
        };
    }

    internal static Control.Key Key(this KeyEventArgs e)
    {
        return e.KeyCode switch
        {
            Keys.ControlKey => Control.Key.Ctrl,
            Keys.Menu => Control.Key.Alt,
            Keys.ShiftKey => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }

    internal static Bitmap GetBitmap(this Plot plot, int width, int height)
    {
        byte[] bytes = plot.GetImage(width, height).GetImageBytes();
        using MemoryStream ms = new MemoryStream(bytes);

        return new Bitmap(ms);
    }

    public static Bitmap GetBitmap(this Image img)
    {
        using MemoryStream ms = new MemoryStream(img.GetImageBytes(ImageFormat.Bmp));

        return new Bitmap(ms);
    }

    public static void CopyToClipboard(this SavedImageInfo info)
    {
        Bitmap bmp = new Bitmap(info.Path);
        Clipboard.SetImage(bmp);
    }
}
