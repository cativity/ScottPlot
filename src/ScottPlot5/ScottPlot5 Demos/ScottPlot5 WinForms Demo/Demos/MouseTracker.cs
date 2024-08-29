using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class MouseTracker : Form, IDemoWindow
{
    public string Title => "Mouse Tracker";

    public string Description => "Demonstrates how to interact with the mouse and convert between screen units (pixels) and axis units (coordinates)";

    public MouseTracker()
    {
        InitializeComponent();

        Crosshair ch = formsPlot1.Plot.Add.Crosshair(0, 0);
        ch.TextColor = Colors.White;
        ch.TextBackgroundColor = ch.HorizontalLine.Color;

        formsPlot1.Refresh();

        formsPlot1.MouseMove += (_, e) =>
        {
            Pixel mousePixel = new Pixel(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);
            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
            ch.Position = mouseCoordinates;
            ch.VerticalLine.Text = $"{mouseCoordinates.X:N3}";
            ch.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
            formsPlot1.Refresh();
        };

        formsPlot1.MouseDown += (_, e) =>
        {
            Pixel mousePixel = new Pixel(e.X, e.Y);
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel);
            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} (mouse down)";
        };
    }
}
