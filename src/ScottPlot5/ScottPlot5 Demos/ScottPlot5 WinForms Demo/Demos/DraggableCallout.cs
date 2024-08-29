using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableCallout : Form, IDemoWindow
{
    public string Title => "Draggable Callout";

    public string Description => "Demonstrates how to make a Callout mouse-interactive";

    private Callout? _calloutBeingDragged;
    private PixelOffset _mouseDownOffset;

    public DraggableCallout()
    {
        InitializeComponent();

        FunctionPlot fp = formsPlot1.Plot.Add.Function(SampleData.DunningKrugerCurve);
        fp.MinX = 0;
        fp.MaxX = 2;
        fp.LineWidth = 3;

        formsPlot1.Plot.Add.Callout("Peak of \"Mount Stupid\"", new Coordinates(0.35, 1.05), new Coordinates(0.2185, fp.GetY(0.2185)));

        formsPlot1.Plot.Add.Callout("Valley of Despair", new Coordinates(0.35, 0.6), new Coordinates(0.3885, fp.GetY(0.3885)));

        formsPlot1.Plot.Add.Callout("Slope of Enlightenment", new Coordinates(0.9, 0.3), new Coordinates(0.76935, fp.GetY(0.76935)));

        formsPlot1.Plot.Add.Callout("Plateau of Sustainability", new Coordinates(1.4, 0.8), new Coordinates(1.701, fp.GetY(1.701)));

        formsPlot1.Plot.YLabel("Confidence");
        formsPlot1.Plot.XLabel("Competence");
        formsPlot1.Plot.Title("Dunning-Kruger Effect", 24);

        formsPlot1.Plot.Axes.SetLimitsX(0, 2);
        formsPlot1.Plot.Axes.SetLimitsY(0, 1.2);

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_calloutBeingDragged is null)
        {
            if (GetCalloutUnderMouse(e.X, e.Y) is Callout calloutUnderMouse)
            {
                Cursor = Cursors.Hand;
                formsPlot1.Plot.MoveToFront(calloutUnderMouse);
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }
        else
        {
            Pixel mousePixel = new Pixel(e.X + _mouseDownOffset.X, e.Y + _mouseDownOffset.Y);
            _calloutBeingDragged.TextCoordinates = _calloutBeingDragged.Axes.GetCoordinates(mousePixel);
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        _calloutBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Callout? calloutUnderMouse = GetCalloutUnderMouse(e.X, e.Y);

        if (calloutUnderMouse is null)
        {
            return;
        }

        _calloutBeingDragged = calloutUnderMouse;

        float dX = calloutUnderMouse.TextPixel.X - e.X;
        float dY = calloutUnderMouse.TextPixel.Y - e.Y;
        _mouseDownOffset = new PixelOffset(dX, dY);

        formsPlot1.Interaction.Disable();
        FormsPlot1_MouseMove(sender, e);
    }

    private Callout? GetCalloutUnderMouse(float x, float y)
    {
        return formsPlot1.Plot.GetPlottables<Callout>().Reverse().FirstOrDefault(p => p.LastRenderRect.Contains(x, y));
    }
}
