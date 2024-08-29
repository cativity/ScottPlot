using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class ShowValueOnHover : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, Scatter";

    public string Description
        => "How to sense where the mouse is in coordinate space and retrieve information about the plotted data the cursor is hovering over";

    public ShowValueOnHover()
    {
        InitializeComponent();

        double[] xs = Generate.RandomSample(100);
        double[] ys = Generate.RandomSample(100);

        Scatter myScatter = formsPlot1.Plot.Add.Scatter(xs, ys);
        myScatter.LineWidth = 0;

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
                                    ? myScatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender)
                                    : myScatter.Data.GetNearestX(mouseLocation, formsPlot1.Plot.LastRender);

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
