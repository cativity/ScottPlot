using JetBrains.Annotations;
using ScottPlot.Palettes;

namespace ScottPlotCookbook.Recipes.PlotTypes;

[UsedImplicitly]
public class Marker : ICategory
{
    public string Chapter => "Plot Types";

    public string CategoryName => "Marker";

    public string CategoryDescription => "Markers can be placed on the plot in coordinate space.";

    public class MarkerQuickstart : RecipeBase
    {
        public override string Name => "Marker Quickstart";

        public override string Description
            => "Markers are symbols placed at a location in coordinate space. Their shape, size, and color can be customized.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            MyPlot.Add.Marker(25, .5);
            MyPlot.Add.Marker(35, .6);
            MyPlot.Add.Marker(45, .7);
        }
    }

    public class MarkerShapes : RecipeBase
    {
        public override string Name => "Marker Shapes";

        public override string Description => "Standard marker shapes are provided, but advanced users are able to create their own as well.";

        [Test]
        public override void Execute()
        {
            MarkerShape[] markerShapes = [.. Enum.GetValues<MarkerShape>()];
            Category20 palette = new Category20();

            for (int i = 0; i < markerShapes.Length; i++)
            {
                ScottPlot.Plottables.Marker mp = MyPlot.Add.Marker(i, 0);
                mp.MarkerStyle.Shape = markerShapes[i];
                mp.MarkerStyle.Size = 10;

                // markers made from filled shapes have can be customized
                mp.MarkerStyle.FillColor = palette.GetColor(i).WithAlpha(.5);

                // markers made from filled shapes have optional outlines
                mp.MarkerStyle.OutlineColor = palette.GetColor(i);
                mp.MarkerStyle.OutlineWidth = 2;

                // markers created from lines can be customized
                mp.MarkerStyle.LineWidth = 2f;
                mp.MarkerStyle.LineColor = palette.GetColor(i);

                ScottPlot.Plottables.Text txt = MyPlot.Add.Text(markerShapes[i].ToString(), i, 0.15);
                txt.LabelRotation = -90;
                txt.LabelAlignment = Alignment.MiddleLeft;
                txt.LabelFontColor = Colors.Black;
            }

            MyPlot.Title("Marker Names");
            MyPlot.Axes.SetLimits(-1, markerShapes.Length, -1, 4);
            MyPlot.HideGrid();
        }
    }

    public class MarkerLegend : RecipeBase
    {
        public override string Name => "Marker Legend";

        public override string Description => "Markers with labels appear in the legend.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Plottables.Signal sin = MyPlot.Add.Signal(Generate.Sin());
            sin.LegendText = "Sine";

            ScottPlot.Plottables.Signal cos = MyPlot.Add.Signal(Generate.Cos());
            cos.LegendText = "Cosine";

            ScottPlot.Plottables.Marker marker = MyPlot.Add.Marker(25, .5);
            marker.LegendText = "Marker";
            MyPlot.ShowLegend();
        }
    }

    public class MarkersPlot : RecipeBase
    {
        public override string Name => "Many Markers";

        public override string Description => "Collections of markers that are all styled the same may be added to the plot";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            MyPlot.Add.Markers(xs, sin, MarkerShape.OpenCircle, 15, Colors.Green);
            MyPlot.Add.Markers(xs, cos, MarkerShape.FilledDiamond, 10, Colors.Magenta);
        }
    }

    public class ImageMarkerQuickstart : RecipeBase
    {
        public override string Name => "Image Marker";

        public override string Description => "An ImageMarker can be placed on the plot to display an image centered at a location in coordinate space.";

        [Test]
        public override void Execute()
        {
            MyPlot.Add.Signal(Generate.Sin());
            MyPlot.Add.Signal(Generate.Cos());

            // An image can be loaded from a file or created dynamically
            Image image = SampleImages.ScottPlotLogo(48, 48);

            Coordinates location1 = new Coordinates(5, .5);
            Coordinates location2 = new Coordinates(25, .5);

            MyPlot.Add.ImageMarker(location1, image);
            MyPlot.Add.ImageMarker(location2, image, 2);

            ScottPlot.Plottables.Marker m1 = MyPlot.Add.Marker(location1);
            ScottPlot.Plottables.Marker m2 = MyPlot.Add.Marker(location2);
            m1.Color = Colors.Orange;
            m2.Color = Colors.Orange;
        }
    }
}
