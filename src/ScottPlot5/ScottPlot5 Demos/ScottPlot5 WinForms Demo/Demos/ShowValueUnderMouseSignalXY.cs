using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class ShowValueUnderMouseSignalXY : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, SignalXY";

    public string Description => "Demonstrates how to determine where the cursor is in coordinate space and identify the data point closest to it.";

    public ShowValueUnderMouseSignalXY()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(1000);
        double[] ys = Generate.RandomWalk(1000);
        SignalXY mySignal = formsPlot1.Plot.Add.SignalXY(xs, ys);

        Crosshair myCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        myCrosshair.IsVisible = false;
        myCrosshair.MarkerShape = MarkerShape.OpenCircle;
        myCrosshair.MarkerSize = 15;

        formsPlot1.MouseMove += (_, e) =>
        {
            // determine where the mouse is and get the nearest point
            Pixel mousePixel = new Pixel(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);

            DataPoint nearest = rbNearestXY.Checked
                                    ? mySignal.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                                    : mySignal.Data.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);

            // place the crosshair over the highlighted point
            if (nearest.IsReal)
            {
                myCrosshair.IsVisible = true;
                myCrosshair.Position = nearest.Coordinates;
                formsPlot1.Refresh();
                Text = $"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}";
            }

            // hide the crosshair when no point is selected
            if (!nearest.IsReal && myCrosshair.IsVisible)
            {
                myCrosshair.IsVisible = false;
                formsPlot1.Refresh();
                Text = "No point selected";
            }
        };
    }
}
