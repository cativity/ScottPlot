using JetBrains.Annotations;
using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

[UsedImplicitly]
public partial class ShowValueOnHoverMultiple : Form, IDemoWindow
{
    public string Title => "Show Value Under Mouse, Multiple Scatter";

    public string Description
        => "How to sense where the mouse is in coordinate space and retrieve information about the plottable and data the cursor is hovering over";

    private readonly List<Scatter> _myScatters = [];

    public ShowValueOnHoverMultiple()
    {
        InitializeComponent();

        // create 3 scatter plots with random points
        for (int i = 0; i < 3; i++)
        {
            double[] xs = Generate.RandomSample(30);
            double[] ys = Generate.RandomSample(30);
            Scatter scatter = formsPlot1.Plot.Add.ScatterPoints(xs, ys);
            scatter.LegendText = $"Scatter {i}";
            scatter.MarkerStyle.Size = 10;
            _myScatters.Add(scatter);
        }

        formsPlot1.Plot.ShowLegend();

        // Create a marker to highlight the point under the cursor
        Crosshair myCrosshair = formsPlot1.Plot.Add.Crosshair(0, 0);
        Marker myHighlightMarker = formsPlot1.Plot.Add.Marker(0, 0);
        myHighlightMarker.Shape = MarkerShape.OpenCircle;
        myHighlightMarker.Size = 17;
        myHighlightMarker.LineWidth = 2;

        // Create a text label to place near the highlighted value
        Text myHighlightText = formsPlot1.Plot.Add.Text("", 0, 0);
        myHighlightText.LabelAlignment = Alignment.LowerLeft;
        myHighlightText.LabelBold = true;
        myHighlightText.OffsetX = 7;
        myHighlightText.OffsetY = -7;

        // Render the plot
        formsPlot1.Refresh();

        // Evaluate points every time the mouse moves.
        // Indicate the nearest point by modifying the crosshair, text, marker, and window title.
        formsPlot1.MouseMove += (_, e) =>
        {
            // determine where the mouse is
            Pixel mousePixel = new Pixel(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);

            // get the nearest point of each scatter
            Dictionary<int, DataPoint> nearestPoints = [];

            for (int i = 0; i < _myScatters.Count; i++)
            {
                DataPoint nearestPoint = _myScatters[i].Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
                nearestPoints.Add(i, nearestPoint);
            }

            // determine which scatter's nearest point is nearest to the mouse
            bool pointSelected = false;
            int scatterIndex = -1;
            double smallestDistance = double.MaxValue;

            for (int i = 0; i < nearestPoints.Count; i++)
            {
                if (nearestPoints[i].IsReal)
                {
                    // calculate the distance of the point to the mouse
                    double distance = nearestPoints[i].Coordinates.Distance(mouseLocation);

                    if (distance < smallestDistance)
                    {
                        // store the index
                        scatterIndex = i;
                        pointSelected = true;
                        // update the smallest distance
                        smallestDistance = distance;
                    }
                }
            }

            // place the crosshair, marker and text over the selected point
            if (pointSelected)
            {
                Scatter scatter = _myScatters[scatterIndex];
                DataPoint point = nearestPoints[scatterIndex];

                myCrosshair.IsVisible = true;
                myCrosshair.Position = point.Coordinates;
                myCrosshair.LineColor = scatter.MarkerStyle.FillColor;

                myHighlightMarker.IsVisible = true;
                myHighlightMarker.Location = point.Coordinates;
                myHighlightMarker.MarkerStyle.LineColor = scatter.MarkerStyle.FillColor;

                myHighlightText.IsVisible = true;
                myHighlightText.Location = point.Coordinates;
                myHighlightText.LabelText = $"{point.X:0.##}, {point.Y:0.##}";
                myHighlightText.LabelFontColor = scatter.MarkerStyle.FillColor;

                formsPlot1.Refresh();
                base.Text = $"Selected Scatter={scatter.LegendText}, Index={point.Index}, X={point.X:0.##}, Y={point.Y:0.##}";
            }

            // hide the crosshair, marker and text when no point is selected
            if (!pointSelected && myCrosshair.IsVisible)
            {
                myCrosshair.IsVisible = false;
                myHighlightMarker.IsVisible = false;
                myHighlightText.IsVisible = false;
                formsPlot1.Refresh();
                base.Text = "No point selected";
            }
        };
    }
}
